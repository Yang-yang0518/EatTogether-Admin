namespace EatTogether.Models.DTOs
{
    public class SetMealItemOptionDto
    {
        public int DishId { get; set; }
        public int ProductId { get; set; }   // Products.Id (FK 對應用)
        public string DishName { get; set; }
        public int Qty { get; set; }
    }
}
