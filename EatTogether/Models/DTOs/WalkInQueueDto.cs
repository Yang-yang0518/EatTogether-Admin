namespace EatTogether.Models.DTOs
{
    public class WalkInQueueDto
    {
        public int Id { get; set; }
        public string QueueNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }
        public int Status { get; set; }
        public string? Remark { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? CalledAt { get; set; }
        public DateTime? SeatedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public int? MemberId { get; set; }
        public int? TableId { get; set; }
        public string? TableName { get; set; }

        public int TotalCount => AdultsCount + ChildrenCount;

        public string StatusText => Status switch
        {
            0 => "等待中",
            1 => "已叫號",
            2 => "已入座",
            3 => "已離開",
            4 => "已過號",
            _ => "未知"
        };

        public string StatusBadgeClass => Status switch
        {
            0 => "bg-primary",
            1 => "bg-warning text-dark",
            2 => "bg-success",
            3 => "bg-secondary",
            4 => "bg-danger",
            _ => "bg-light text-dark"
        };
    }
}
