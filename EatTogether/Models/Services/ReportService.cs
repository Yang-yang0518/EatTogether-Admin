using EatTogether.Models.DTOs;
using EatTogether.Models.Repositories;
using EatTogether.Models.ViewModels;

namespace EatTogether.Models.Services
{
    public interface IReportService
    {
        Task<ReportViewModel> GetReportAsync(ReportQueryDto query);
        Task<DateTime?> GetEarliestOrderDateAsync();
    }

    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepo;
        public ReportService(IReportRepository reportRepo) => _reportRepo = reportRepo;

        public async Task<DateTime?> GetEarliestOrderDateAsync()
        {
            var orders = await _reportRepo.GetCompletedOrdersAsync(
                new DateTime(2000, 1, 1), DateTime.Today);
            if (!orders.Any()) return null;
            return orders.Min(o => o.OrderAt).Date;
        }

        public async Task<ReportViewModel> GetReportAsync(ReportQueryDto query)
        {
            var start = query.StartDate;
            var end = query.EndDate;

            // Orders = 正式成交資料（付款成功）
            // PreOrders = 全部訂單，用於計算取消率
            var orders = await _reportRepo.GetCompletedOrdersAsync(start, end);
            var allPre = await _reportRepo.GetAllPreOrdersInRangeAsync(start, end);
            var topProducts = await _reportRepo.GetTopProductsAsync(start, end, 10);
            var couponRanking = await _reportRepo.GetCouponRankingAsync(start, end);

            // ── KPI ──────────────────────────────────────────────────────────
            int totalRevenue = orders.Sum(o => o.TotalAmount);
            int totalOrders = orders.Count;
            int totalAll = allPre.Count;
            int cancelled = allPre.Count(p => p.DoneOrCancel == 2);
            int pending = allPre.Count(p => p.DoneOrCancel == 0);

            double avgOrder = totalOrders > 0 ? (double)totalRevenue / totalOrders : 0;
            double cancelRate = totalAll > 0 ? (double)cancelled / totalAll * 100 : 0;

            // ── 趨勢 ─────────────────────────────────────────────────────────
            var trendLabels = new List<string>();
            var revenueTrend = new List<int>();
            var orderTrend = new List<int>();

            switch (query.Period)
            {
                case "day":
                    for (int h = 0; h < 24; h++)
                    {
                        trendLabels.Add($"{h:D2}:00");
                        var g = orders.Where(o => o.OrderAt.Hour == h).ToList();
                        revenueTrend.Add(g.Sum(o => o.TotalAmount));
                        orderTrend.Add(g.Count);
                    }
                    break;

                case "week":
                    var dayNames = new[] { "日", "一", "二", "三", "四", "五", "六" };
                    for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
                    {
                        trendLabels.Add($"{d:MM/dd}（{dayNames[(int)d.DayOfWeek]}）");
                        var g = orders.Where(o => o.OrderAt.Date == d).ToList();
                        revenueTrend.Add(g.Sum(o => o.TotalAmount));
                        orderTrend.Add(g.Count);
                    }
                    break;

                case "month":
                    for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
                    {
                        trendLabels.Add(d.ToString("MM/dd"));
                        var g = orders.Where(o => o.OrderAt.Date == d).ToList();
                        revenueTrend.Add(g.Sum(o => o.TotalAmount));
                        orderTrend.Add(g.Count);
                    }
                    break;

                case "quarter":
                    var cur = start.Date;
                    while (cur <= end.Date)
                    {
                        var weekEnd = cur.AddDays(6) > end.Date ? end.Date : cur.AddDays(6);
                        trendLabels.Add($"{cur:MM/dd}");
                        var g = orders
                            .Where(o => o.OrderAt.Date >= cur && o.OrderAt.Date <= weekEnd)
                            .ToList();
                        revenueTrend.Add(g.Sum(o => o.TotalAmount));
                        orderTrend.Add(g.Count);
                        cur = cur.AddDays(7);
                    }
                    break;

                case "year":
                    for (int m = 1; m <= 12; m++)
                    {
                        trendLabels.Add($"{start.Year}/{m:D2}");
                        var g = orders.Where(o => o.OrderAt.Month == m).ToList();
                        revenueTrend.Add(g.Sum(o => o.TotalAmount));
                        orderTrend.Add(g.Count);
                    }
                    break;

                case "all":
                    // 全部：依跨越年數決定分組，跨 1 年內用月，超過用季
                    int totalMonths = (end.Year - start.Year) * 12 + end.Month - start.Month + 1;
                    if (totalMonths <= 24)
                    {
                        // 每月一點
                        var mCur2 = new DateTime(start.Year, start.Month, 1);
                        while (mCur2 <= end)
                        {
                            trendLabels.Add(mCur2.ToString("yyyy/MM"));
                            var g = orders.Where(o => o.OrderAt.Year == mCur2.Year && o.OrderAt.Month == mCur2.Month).ToList();
                            revenueTrend.Add(g.Sum(o => o.TotalAmount));
                            orderTrend.Add(g.Count);
                            mCur2 = mCur2.AddMonths(1);
                        }
                    }
                    else
                    {
                        // 每季一點
                        var qCur = new DateTime(start.Year, ((start.Month - 1) / 3) * 3 + 1, 1);
                        while (qCur <= end)
                        {
                            int qNum = (qCur.Month - 1) / 3 + 1;
                            trendLabels.Add($"{qCur.Year}/Q{qNum}");
                            var qEnd = qCur.AddMonths(3).AddDays(-1);
                            var g = orders.Where(o => o.OrderAt >= qCur && o.OrderAt <= qEnd).ToList();
                            revenueTrend.Add(g.Sum(o => o.TotalAmount));
                            orderTrend.Add(g.Count);
                            qCur = qCur.AddMonths(3);
                        }
                    }
                    break;

                default:
                    for (int m = 1; m <= 12; m++)
                    {
                        trendLabels.Add($"{start.Year}/{m:D2}");
                        var gd = orders.Where(o => o.OrderAt.Month == m).ToList();
                        revenueTrend.Add(gd.Sum(o => o.TotalAmount));
                        orderTrend.Add(gd.Count);
                    }
                    break;

                case "custom":
                    // 依區間長度自動決定分組：≤31天 → 每天，≤180天 → 每週，其他 → 每月
                    int spanDays = (int)(end - start).TotalDays + 1;
                    if (spanDays <= 31)
                    {
                        for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
                        {
                            trendLabels.Add(d.ToString("MM/dd"));
                            var g = orders.Where(o => o.OrderAt.Date == d).ToList();
                            revenueTrend.Add(g.Sum(o => o.TotalAmount));
                            orderTrend.Add(g.Count);
                        }
                    }
                    else if (spanDays <= 180)
                    {
                        var wCur = start.Date;
                        while (wCur <= end.Date)
                        {
                            var wEnd = wCur.AddDays(6) > end.Date ? end.Date : wCur.AddDays(6);
                            trendLabels.Add($"{wCur:MM/dd}");
                            var g = orders.Where(o => o.OrderAt.Date >= wCur && o.OrderAt.Date <= wEnd).ToList();
                            revenueTrend.Add(g.Sum(o => o.TotalAmount));
                            orderTrend.Add(g.Count);
                            wCur = wCur.AddDays(7);
                        }
                    }
                    else
                    {
                        var mCur = new DateTime(start.Year, start.Month, 1);
                        while (mCur <= end)
                        {
                            trendLabels.Add(mCur.ToString("yyyy/MM"));
                            var g = orders.Where(o => o.OrderAt.Year == mCur.Year && o.OrderAt.Month == mCur.Month).ToList();
                            revenueTrend.Add(g.Sum(o => o.TotalAmount));
                            orderTrend.Add(g.Count);
                            mCur = mCur.AddMonths(1);
                        }
                    }
                    break;
            }

            // ── 付款方式（從 Orders 撈，排除空白）────────────────────────────
            static string ToPayLabel(string? m) => m?.Trim() switch
            {
                "Card" or "Credit Card" => "信用卡",
                "Cash"                  => "現金",
                "LinePay" or "Line Pay" => "行動支付",
                _                       => m ?? ""
            };

            var payGroups = orders
                .Where(o => !string.IsNullOrWhiteSpace(o.PayMethod))
                .GroupBy(o => ToPayLabel(o.PayMethod))
                .OrderByDescending(g => g.Count())
                .ToList();

            // ── 內用 vs 外帶 ─────────────────────────────────────────────────
            int dineInCount = orders.Count(o => o.InOrOut);
            int takeoutCount = orders.Count(o => !o.InOrOut);
            int dineInRevenue = orders.Where(o => o.InOrOut).Sum(o => o.TotalAmount);
            int takeoutRevenue = orders.Where(o => !o.InOrOut).Sum(o => o.TotalAmount);

            // ── 優惠券 ────────────────────────────────────────────────────────
            int ordersWithCoupon = orders.Count(o => o.CouponId.HasValue);
            int totalDiscount = orders.Sum(o => o.DiscountAmount);

            return new ReportViewModel
            {
                Period = query.Period,
                StartDate = start,
                EndDate = end,

                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalGuests = totalOrders,
                AvgOrderValue = Math.Round(avgOrder, 0),
                CancelledOrders = cancelled,
                CancelRate = Math.Round(cancelRate, 1),

                TrendLabels = trendLabels,
                RevenueTrendData = revenueTrend,
                OrderCountTrendData = orderTrend,

                PayMethodLabels = payGroups.Select(g => g.Key).ToList(),
                PayMethodData = payGroups.Select(g => g.Count()).ToList(),
                PayMethodRevenue = payGroups.Select(g => g.Sum(o => o.TotalAmount)).ToList(),

                DineInCount = dineInCount,
                TakeoutCount = takeoutCount,
                DineInRevenue = dineInRevenue,
                TakeoutRevenue = takeoutRevenue,

                TopProducts = topProducts,

                // 訂單狀態：成交數從 Orders，取消/待處理從 PreOrders
                StatusDone = totalOrders,
                StatusCancelled = cancelled,
                StatusPending = pending,

                OrdersWithCoupon = ordersWithCoupon,
                TotalDiscount = totalDiscount,
                OrdersNoCoupon = totalOrders - ordersWithCoupon,
                CouponRanking = couponRanking,
            };
        }
    }
}
