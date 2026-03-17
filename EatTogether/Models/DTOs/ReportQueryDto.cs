namespace EatTogether.Models.DTOs
{
    public class ReportQueryDto
    {
        public string Period { get; set; } = "month";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
