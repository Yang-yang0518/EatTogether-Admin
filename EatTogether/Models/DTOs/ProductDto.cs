namespace EatTogether.Models.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        // "Dish" 或 "SetMeal"
        public string ProductType { get; set; } = null!;

        // 單點餐點資訊
        public int? DishId { get; set; }
        public string? DishName { get; set; }
        public decimal? DishPrice { get; set; }
        public string? DishImageUrl { get; set; }
        public string? DishCategoryName { get; set; }

        // 套餐資訊
        public int? SetMealId { get; set; }
        public string? SetMealName { get; set; }
        public decimal? SetMealPrice { get; set; }
        public string? SetMealImageUrl { get; set; }

        public string? DishDescription { get; set; }
        public bool DishIsRecommended { get; set; }
        public bool DishIsVegetarian { get; set; }
        public int DishSpicyLevel { get; set; }
        public bool DishIsPopular { get; set; }

        // 統一顯示用（不管單點或套餐都可以用這兩個）
        public string DisplayName => ProductType == "Dish" ? DishName! : SetMealName!;
        public decimal? DisplayPrice => ProductType == "Dish" ? DishPrice : SetMealPrice;
        public string? DisplayImageUrl => ProductType == "Dish" ? DishImageUrl : SetMealImageUrl;
    }
}
