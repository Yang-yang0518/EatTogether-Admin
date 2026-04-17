using EatTogether.Models.DTOs;
using EatTogether.Models.Infra;
using EatTogether.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EatTogether.Controllers
{
	[RequirePermission("Order_Manage")]
	public class PaymentsController : Controller
    {
        private readonly IOrderService _service;
        private readonly EcPayService  _ecPay;
        private readonly IMemoryCache  _cache;

        public PaymentsController(IOrderService service, EcPayService ecPay, IMemoryCache cache)
        {
            _service = service;
            _ecPay   = ecPay;
            _cache   = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? tableId, bool success = false)
        {
            var vm = await _service.GetPaymentIndexAsync();
            ViewBag.DefaultTableId = tableId ?? 0;
            ViewBag.ShowSuccess = success;  // ← 加這行
            return View(vm);
        }

        [HttpGet]
        [Route("Payments/GetDetail")]
        public async Task<IActionResult> GetDetail(int preOrderId)
        {
            var vm = await _service.GetCheckoutDetailAsync(preOrderId);
            if (vm == null) return NotFound();
            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelUnserved(int preOrderId)
        {
            await _service.CancelUnservedDetailsAsync(preOrderId);
            var vm = await _service.GetCheckoutDetailAsync(preOrderId);
            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(int preOrderId, string payMethod)
        {
            await _service.CheckoutAsync(preOrderId, payMethod);
            return RedirectToAction(nameof(Create), new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> GetDetailByTable(int tableId)
        {
            var vm = await _service.GetCheckoutByTableAsync(tableId);
            if (vm == null) return NotFound();
            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelUnservedByTable(int tableId)
        {
            await _service.CancelUnservedByTableAsync(tableId);
            var vm = await _service.GetCheckoutByTableAsync(tableId);
            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutByTable(int tableId, string payMethod)
        {
            await _service.CheckoutByTableAsync(tableId, payMethod);
            return RedirectToAction(nameof(Create), new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetSplitEvents(int amount)
            => Json(await _service.GetEventsForSplitAsync(amount));

        [HttpGet]
        public async Task<IActionResult> GetSplitCoupons(int amount, int? memberId = null)
            => Json(await _service.GetCouponsForSplitAsync(amount, memberId));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SplitCheckout([FromBody] SplitCheckoutRequestDto dto)
        {
            // 執行拆單結帳邏輯
            await _service.SplitCheckoutAsync(dto.DetailIds, dto.PayMethod,
                dto.MemberId, dto.CouponId, dto.EventId);

            // 重新取得該桌目前「未結帳」的剩餘餐點
            var remaining = await _service.GetCheckoutByTableAsync(dto.TableId);

            // 如果 remaining 為 null，代表該訂單已完全結清
            return Json(new
            {
                success = true,
                remaining = remaining,
                isFullyPaid = (remaining == null || remaining.Items.Count == 0)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderTable(int preOrderId, int? tableId, bool inOrOut)
        {
            await _service.UpdateOrderTableAsync(preOrderId, tableId, inOrOut);
            var vm = await _service.GetCheckoutDetailAsync(preOrderId);
            return Json(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AvailableTables()
        {
            var vm = await _service.GetPaymentIndexAsync();
            var tables = vm.Tables
                .Where(t => !t.HasOrder && !t.IsOccupied)
                .Select(t => new { t.TableId, t.TableName })
                .ToList();
            return Json(tables);
        }

        [HttpGet]
        public async Task<IActionResult> GetManualEvents(int? tableId, int? preOrderId)
        {
            var events = await _service.GetManualEventsForOrderAsync(tableId, preOrderId);
            return Json(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyEvent(int? tableId, int? preOrderId, int? eventId)
        {
            var (success, error, vm) = await _service.ApplyEventToOrderAsync(tableId, preOrderId, eventId);
            if (!success) return Json(new { success = false, error });
            return Json(new { success = true, data = vm });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyCoupon(int? tableId, int? preOrderId, string couponCode)
        {
            var (success, error, vm) = await _service.ApplyCouponToOrderAsync(tableId, preOrderId, couponCode);
            if (!success) return Json(new { success = false, error });
            return Json(new { success = true, data = vm });
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicableCoupons(int? tableId, int? preOrderId)
        {
            var coupons = await _service.GetApplicableCouponsForOrderAsync(tableId, preOrderId);
            return Json(coupons);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyCouponById(int? tableId, int? preOrderId, int? couponId)
        {
            try
            {
                var vm = await _service.ApplyCouponByIdToOrderAsync(tableId, preOrderId, couponId);
                if (vm == null) return Json(new { success = false, error = "找不到訂單" });
                return Json(new { success = true, data = vm });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"套用失敗：{ex.Message}" });
            }
        }

        // ── 綠界信用卡結帳 ──────────────────────────────────────────────────

        /// <summary>前端 AJAX 呼叫：取得送往綠界的表單參數（不直接結帳）</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InitiateCardPayment(int? tableId, int? preOrderId)
        {
            // 取得金額
            int amount;
            string orderNo;
            string tradeNo;

            if (tableId.HasValue)
            {
                var vm = await _service.GetCheckoutByTableAsync(tableId.Value);
                if (vm == null) return Json(new { success = false, error = "找不到訂單" });
                amount  = vm.TotalAmount;
                orderNo = vm.OrderNumber;
                tradeNo = EcPayService.MakeTradeNo(tableId.Value, isTable: true);
            }
            else if (preOrderId.HasValue)
            {
                var vm = await _service.GetCheckoutDetailAsync(preOrderId.Value);
                if (vm == null) return Json(new { success = false, error = "找不到訂單" });
                amount  = vm.TotalAmount;
                orderNo = vm.OrderNumber;
                tradeNo = EcPayService.MakeTradeNo(preOrderId.Value, isTable: false);
            }
            else
            {
                return Json(new { success = false, error = "參數錯誤" });
            }

            if (amount <= 0)
                return Json(new { success = false, error = "應付金額為 0，無需刷卡" });

            var itemName     = $"EatTogether Order {orderNo}";
            var tradeDesc    = "EatTogether";
            var clientBackUrl = _ecPay.ClientBackUrl + "?tradeNo=" + tradeNo;
            var formParams   = _ecPay.BuildParams(tradeNo, amount, itemName, tradeDesc, clientBackUrl);

            return Json(new
            {
                success    = true,
                paymentUrl = _ecPay.PaymentUrl,
                formParams
            });
        }

        /// <summary>拆單刷卡：暫存 detailIds，回傳綠界表單參數</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InitiateSplitCardPayment([FromBody] SplitCheckoutRequestDto dto)
        {
            if (dto.DetailIds == null || !dto.DetailIds.Any())
                return Json(new { success = false, error = "未選擇任何餐點" });

            // 從 VM 取得已選餐點金額
            var vm = await _service.GetCheckoutByTableAsync(dto.TableId);
            if (vm == null) return Json(new { success = false, error = "找不到訂單" });

            var originalAmt = vm.Items
                .Where(i => dto.DetailIds.Contains(i.DetailId) && !i.ParentDetailId.HasValue)
                .Sum(i => i.SubTotal);

            if (originalAmt <= 0)
                return Json(new { success = false, error = "應付金額為 0，無需刷卡" });

            // 計算折扣後應付金額（與 SplitCheckoutAsync 邏輯一致）
            int couponDisc = 0, eventDisc = 0;
            if (dto.CouponId.HasValue)
            {
                var coupons = await _service.GetCouponsForSplitAsync(originalAmt);
                var c = coupons.FirstOrDefault(x => x.Id == dto.CouponId.Value);
                if (c != null && c.IsEligible)
                    couponDisc = c.DiscountType == 0 ? c.DiscountValue
                        : (int)(originalAmt * c.DiscountValue / 100m);
            }
            if (dto.EventId.HasValue)
            {
                var events = await _service.GetEventsForSplitAsync(originalAmt);
                var e = events.FirstOrDefault(x => x.Id == dto.EventId.Value);
                if (e != null && e.IsEligible && e.DiscountType != "Gift")
                    eventDisc = e.DiscountType == "FixedAmount" ? (int)e.DiscountValue
                        : (int)(originalAmt * e.DiscountValue / 100m);
            }
            var amount = Math.Max(1, originalAmt - couponDisc - eventDisc);

            var tradeNo = EcPayService.MakeSplitTradeNo(dto.TableId);

            // 暫存拆單資料（30 分鐘有效）
            _cache.Set($"split:{tradeNo}", dto,
                       TimeSpan.FromMinutes(30));

            var clientBackUrl = _ecPay.ClientBackUrl + "?tradeNo=" + tradeNo;
            var formParams = _ecPay.BuildParams(
                tradeNo, amount,
                itemName:     $"EatTogether Split {vm.TableName}",
                tradeDesc:    "EatTogether",
                clientBackUrl: clientBackUrl);

            return Json(new { success = true, paymentUrl = _ecPay.PaymentUrl, formParams });
        }

        /// <summary>綠界 Server-to-Server 回呼（不需 AntiForgery）</summary>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> EcPayCallback()
        {
            var form = Request.Form.SelectMany(kv =>
                kv.Value.Select(v => new KeyValuePair<string, string>(kv.Key, v ?? "")));

            if (!_ecPay.VerifyCheckMac(form))
                return Content("0|CheckMacValue Error");

            var rtnCode = Request.Form["RtnCode"].ToString();
            var tradeNo = Request.Form["MerchantTradeNo"].ToString();

            // 將付款結果存入 Cache，供 EcPayReturn 頁面顯示（10 分鐘有效）
            _cache.Set($"ecpay-result:{tradeNo}", rtnCode == "1", TimeSpan.FromMinutes(10));

            if (rtnCode != "1")
                return Content("1|OK");

            try
            {
                var (prefix, id) = EcPayService.ParseTradeNo(tradeNo);
                switch (prefix)
                {
                    case 'T':
                        await _service.CheckoutByTableAsync(id, "Card");
                        break;
                    case 'O':
                        await _service.CheckoutAsync(id, "Card");
                        break;
                    case 'S':
                        if (_cache.TryGetValue($"split:{tradeNo}", out SplitCheckoutRequestDto? splitDto) && splitDto != null)
                        {
                            await _service.SplitCheckoutAsync(splitDto.DetailIds, "Card",
                                splitDto.MemberId, splitDto.CouponId, splitDto.EventId);
                            _cache.Remove($"split:{tradeNo}");
                        }
                        break;
                }
            }
            catch
            {
                // 重複回呼或訂單已結帳時靜默忽略
            }

            return Content("1|OK");
        }

        /// <summary>綠界付款完成後，瀏覽器跳回此頁</summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult EcPayReturn(string? tradeNo)
        {
            ViewBag.TradeNo = tradeNo;
            // 若 cache 已有結果直接顯示；否則讓前端 polling
            if (_cache.TryGetValue($"ecpay-result:{tradeNo}", out bool result))
            {
                ViewBag.Known   = true;
                ViewBag.Success = result;
            }
            else
            {
                ViewBag.Known   = false;
                ViewBag.Success = false;
            }
            return View();
        }

        /// <summary>前端 polling：查詢綠界付款結果是否已寫入 Cache</summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult EcPayStatus(string? tradeNo)
        {
            if (string.IsNullOrWhiteSpace(tradeNo))
                return Json(new { known = false });

            if (_cache.TryGetValue($"ecpay-result:{tradeNo}", out bool result))
                return Json(new { known = true, success = result });

            return Json(new { known = false });
        }

        [HttpGet]
        public async Task<IActionResult> SearchMember(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return Json(new { success = false, error = "請輸入電話號碼" });

            var member = await _service.SearchMemberByPhoneAsync(phone.Trim());
            if (member == null)
                return Json(new { success = false, error = "查無此電話的會員" });

            return Json(new
            {
                success = true,
                member  = new
                {
                    id           = member.Id,
                    name         = member.Name,
                    phone        = member.Phone,
                    isBlacklisted= member.IsBlacklisted,
                    isConfirmed  = member.IsConfirmed
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyMember(int? tableId, int? preOrderId, int? memberId)
        {
            try
            {
                var vm = await _service.ApplyMemberToOrderAsync(tableId, preOrderId, memberId);
                if (vm == null) return Json(new { success = false, error = "找不到訂單" });
                return Json(new { success = true, data = vm });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"套用失敗：{ex.Message}" });
            }
        }
    }
}
