namespace EatTogether.Models.DTOs
{
	public class SetMealItemDto
	{
		// Id SetMealId DishId DishName DishPrice Quantity IsOptional OptionGroupNo PickLimit DisplayOrder
		public int Id { get; set; }
		public int SetMealId { get; set; }
		public int DishId { get; set; }
		public string? DishName { get; set; } 
		public decimal? DishPrice { get; set; }
		public string? CategoryName { get; set; }
		public int Quantity { get; set; }
		public bool IsOptional { get; set; }
		public int? OptionGroupNo { get; set; }
		public int? PickLimit { get; set; }
		public int DisplayOrder { get; set; }
	}

	public class SetMealItemGroupDto
	{
		public int GroupNo { get; set; }
		public int PickLimit { get; set; }
		public bool IsOptional { get; set; }
		public List<SetMealItemOptionDto> Options { get; set; } = new();
	}

	public class SetMealItemOptionDto
	{
		public int DishId { get; set; }
		public int ProductId { get; set; }
		public string DishName { get; set; }
		public int Qty { get; set; }
	}
}
