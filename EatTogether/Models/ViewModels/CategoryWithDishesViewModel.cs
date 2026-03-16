using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EatTogether.Models.ViewModels
{
    public class CategoryWithDishesViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<SelectListItem> DishesInThisCategory { get; set; } = new();

        // Properties for optionality at category level, as per user's mental model
        [Display(Name = "是否為選擇性分類")]
        public bool IsCategoryOptional { get; set; } = false;
        
        [Display(Name = "分類群組編號")]
        public int? OptionGroupNoForCategory { get; set; }
        
        [Display(Name = "分類選取上限")]
        public int? PickLimitForCategory { get; set; }

        // To hold the currently selected items for this category from the SetMeal
        // This will be populated in the controller and used by the frontend to render initial selections
        public List<SetMealItemViewModel> SelectedItemsForCategory { get; set; } = new();
    }
}
