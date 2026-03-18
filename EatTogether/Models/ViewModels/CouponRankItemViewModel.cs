namespace EatTogether.Models.ViewModels
{
    public class CouponRankItemViewModel
    {
        public string CouponName { get; set; } = string.Empty;
        public int UsedCount { get; set; }       // 使用筆數
        public int TotalDiscount { get; set; }   // 折抵總金額
    }
}
