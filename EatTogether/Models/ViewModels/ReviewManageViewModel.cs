namespace EatTogether.Models.ViewModels
{
    public class ReviewManageViewModel
    {
        public int Id { get; set; }
        public string DishName { get; set; } = null!;

        public string Nickname { get; set; } = null!;

        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
