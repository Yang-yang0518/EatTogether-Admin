using System.ComponentModel.DataAnnotations;

namespace EatTogether.Models.ViewModels
{
    public class CreatePreOrderItemViewModel
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }

        public int Qty { get; set; }

        public int UnitPrice { get; set; }
        public bool IsSetMeal { get; set; }
        public int? ParentIndex { get; set; }
        public string? CategoryName { get; set; }

        // 前台顯示用欄位
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsVegetarian { get; set; }
        public int SpicyLevel { get; set; }
        public bool IsPopular { get; set; }
    }
}
