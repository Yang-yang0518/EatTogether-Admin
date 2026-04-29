namespace EatTogether.Models.ViewModels
{
    public class ReviewManageViewModel
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public string Nickname { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
