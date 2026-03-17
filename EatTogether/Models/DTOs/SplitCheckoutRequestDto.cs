namespace EatTogether.Models.DTOs
{
    public class SplitCheckoutRequestDto
    {
        public int TableId { get; set; }
        public List<int> DetailIds { get; set; } = new();
        public string PayMethod { get; set; } = string.Empty;
    }
}
