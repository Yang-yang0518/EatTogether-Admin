using EatTogether.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IOrderService _service;
        public PaymentsController(IOrderService service) => _service = service;

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
    }
}
