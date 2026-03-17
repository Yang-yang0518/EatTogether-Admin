namespace EatTogether.Models.DTOs
{
    public class CouponValidateDto
    {
        public bool IsValid { get; set; }
        public int? CouponId { get; set; }
        public string CouponName { get; set; } = string.Empty;
        public int Discount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
