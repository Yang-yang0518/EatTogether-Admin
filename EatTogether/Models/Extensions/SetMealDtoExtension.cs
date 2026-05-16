using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;

namespace EatTogether.Models.Extensions
{
	public static class SetMealDtoExtension
	{
		public static SetMealDto ToDto(this SetMeal setMeal)
		{
			return new SetMealDto
			{
				Id = setMeal.Id,
				SetMealName = setMeal.SetMealName,
				DiscountType = setMeal.DiscountType,
				DiscountValue = setMeal.DiscountValue,
				IsActive = setMeal.IsActive,
				CreatedAt = setMeal.CreatedAt,
				SetPrice = setMeal.SetPrice,
				Description = setMeal.Description,
				ImageUrl = setMeal.ImageUrl,
				StartDate = setMeal.StartDate,
				EndDate = setMeal.EndDate,
				StartTime = setMeal.StartTime,
				EndTime = setMeal.EndTime,
				UpdatedAt = setMeal.UpdatedAt,
				DisplayOrder = setMeal.DisplayOrder,
				IsPopular = setMeal.IsPopular,
				IsRecommended = setMeal.IsRecommended,
				Items = setMeal.SetMealItems.Select(i => i.ToItemDto()).ToList()
			};
		}
			
			public static SetMealItemDto ToItemDto(this SetMealItem item)
		{
			return new SetMealItemDto
			{
				Id = item.Id,
				SetMealId = item.SetMealId,
				DishId = item.DishId,
				DishName = item.Dish?.DishName,
				DishPrice = item.Dish?.Price,
				CategoryName = item.Dish?.Category?.CategoryName,
				Quantity = item.Quantity,
				IsOptional = item.IsOptional,
				OptionGroupNo = item.OptionGroupNo,
				PickLimit = item.PickLimit,
				DisplayOrder = item.DisplayOrder
			};
		}
	}
}		
