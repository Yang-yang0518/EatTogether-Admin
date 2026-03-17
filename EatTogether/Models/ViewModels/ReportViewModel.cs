namespace EatTogether.Models.ViewModels
{
    public class ReportViewModel
    {
        // ── 篩選條件 ──────────────────────────────────────────────────────────
        public string Period { get; set; } = "month";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // ── 下拉選單選項（Controller 產生） ──────────────────────────────────
        public List<DropdownOption> DayOptions { get; set; } = new();
        public List<DropdownOption> WeekOptions { get; set; } = new();
        public List<DropdownOption> MonthOptions { get; set; } = new();
        public List<DropdownOption> QuarterOptions { get; set; } = new();
        public List<DropdownOption> YearOptions { get; set; } = new();

        // ── KPI ──────────────────────────────────────────────────────────────
        public int TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalGuests { get; set; }
        public double AvgOrderValue { get; set; }
        public int CancelledOrders { get; set; }
        public double CancelRate { get; set; }

        // ── 趨勢圖 ────────────────────────────────────────────────────────────
        public List<string> TrendLabels { get; set; } = new();
        public List<int> RevenueTrendData { get; set; } = new();
        public List<int> OrderCountTrendData { get; set; } = new();

        // ── 付款方式 ─────────────────────────────────────────────────────────
        public List<string> PayMethodLabels { get; set; } = new();
        public List<int> PayMethodData { get; set; } = new();
        public List<int> PayMethodRevenue { get; set; } = new();

        // ── 內用 vs 外帶 ─────────────────────────────────────────────────────
        public int DineInCount { get; set; }
        public int TakeoutCount { get; set; }
        public int DineInRevenue { get; set; }
        public int TakeoutRevenue { get; set; }

        // ── 熱銷商品 ─────────────────────────────────────────────────────────
        public List<ProductRankItemViewModel> TopProducts { get; set; } = new();

        // ── 訂單狀態 ─────────────────────────────────────────────────────────
        public int StatusDone { get; set; }
        public int StatusCancelled { get; set; }
        public int StatusPending { get; set; }

        // ── 優惠券 ────────────────────────────────────────────────────────────
        public int OrdersWithCoupon { get; set; }
        public int TotalDiscount { get; set; }
        public int OrdersNoCoupon { get; set; }
        public List<CouponRankItemViewModel> CouponRanking { get; set; } = new();
    }
    public class DropdownOption
    {
        public string Label { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;  // yyyy-MM-dd
        public string EndDate { get; set; } = string.Empty;    // yyyy-MM-dd
        public bool IsSelected { get; set; }
    }
}
