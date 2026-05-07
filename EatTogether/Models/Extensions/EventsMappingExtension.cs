using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using EatTogether.Models.ViewModels;
using System.Linq.Expressions;

namespace EatTogether.Models.Extensions
{
	public static class EventsMappingExtension
	{
		//活動新增
		public static EventCreateDto ToCreateDto(this EventCreateViewModel vm)
		{
			return new EventCreateDto
			{
				//Id = vm.Id,
				Title = vm.Title,
				Summary = vm.Summary,
				MinSpend = vm.MinSpend.Value,
				StartDate = vm.StartDate.Value,
				EndDate = vm.EndDate.Value,
				RewardDishId = vm.RewardDishId,
				RewardDishName = vm.RewardDishName,
				DiscountType = vm.DiscountType,
				DiscountValue = vm.DiscountValue,
				Status = vm.Status,
				IsAutoDiscount = vm.IsAutoDiscount
			};
		}

		public static Event ToEntity(this EventCreateDto dto)
		{
			return new Event
			{
				Id = dto.Id,
				Title = dto.Title,
				Summary = dto.Summary,
				MinSpend = dto.MinSpend,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate,
				RewardDishId = dto.RewardDishId,
				DiscountType = dto.DiscountType,
				DiscountValue = dto.DiscountValue,
				Status = dto.Status,
				IsAutoDiscount = dto.IsAutoDiscount
			};
		}


		//活動列表 dto -> vm
		//→ ToViewModel(this EventDto dto)
		//// Entity → Dto（Repository 讀取用）
		//EventDto ToDto(this Event entity)

		public static EventViewModel ToEventVm(this EventDto dto)
		{
			return new EventViewModel
			{
				Id = dto.Id,
				Title = dto.Title,
				Summary = dto.Summary,
				MinSpend = dto.MinSpend,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate,
				RewardDishId = dto.RewardDishId,
				RewardDishName = dto.RewardDishName,
				DiscountType = dto.DiscountType,
				DiscountValue = dto.DiscountValue,
				Status = dto.Status
			};
		}

		//提供給 EF Core Select 專用的 Expression(能完美翻譯成 SQL)
		public static Expression<Func<Event, EventDto>> ToEventDtoExpression() {

			return entity => new EventDto
			{
				Id = entity.Id,
				Title = entity.Title,
				Summary = entity.Summary,
				MinSpend = entity.MinSpend,
				StartDate = entity.StartDate,
				EndDate = (DateTime)entity.EndDate,
				RewardDishId = entity.RewardDishId,
				RewardDishName = entity.RewardDish != null ? entity.RewardDish.DishName : null,
				DiscountType = entity.DiscountType,
				DiscountValue = entity.DiscountValue,
				Status = entity.Status
			};
		
		}

		public static EventDto ToEventDto(this Event entity)
		{
			return new EventDto
			{
				Id = entity.Id,
				Title = entity.Title,
				Summary = entity.Summary,
				MinSpend = entity.MinSpend,
				StartDate = entity.StartDate,
				EndDate = (DateTime)entity.EndDate,
				RewardDishId = entity.RewardDishId,
				RewardDishName = entity.RewardDish?.DishName,
				DiscountType = entity.DiscountType,
				DiscountValue = entity.DiscountValue,
				Status = entity.Status
			};
		}


		//活動編輯
		//	vm<-> dto
		//→ ToDto(this EventEditViewModel vm)
		//→ ToViewModel(this AEventEditDto dto)
		//	// Dto → Entity（Repository 寫入用）
		//	→Event ToEntity(this EventEditDto dto)

		//	// Entity → Dto（Repository 讀取用）
		//	EventDto ToDto(this Event entity)

		public static EventEditDto ToEditDto(this EventEditViewModel vm)
		{
			return new EventEditDto
			{
				Id = vm.Id,
				Title = vm.Title,
				Summary = vm.Summary,
				MinSpend = vm.MinSpend.Value,
				StartDate = vm.StartDate.Value,
				EndDate = vm.EndDate.Value,
				RewardDishId= vm.RewardDishId,
				RewardDishName = vm.RewardDishName,
				DiscountType = vm.DiscountType,
				DiscountValue = vm.DiscountValue.Value,
				Status = vm.Status,
				IsAutoDiscount = vm.IsAutoDiscount
			};
		}

		public static EventEditViewModel ToEditVm(this EventEditDto dto)
		{
			return new EventEditViewModel
			{
				Id = dto.Id,
				Title = dto.Title,
				Summary = dto.Summary,
				MinSpend = dto.MinSpend,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate,
				RewardDishId = dto.RewardDishId,
				RewardDishName = dto.RewardDishName,
				DiscountType = dto.DiscountType,
				DiscountValue = dto.DiscountValue,
				Status = dto.Status,
				IsAutoDiscount = dto.IsAutoDiscount
			};
		}

		public static Event ToEntity(this EventEditDto dto)
		{
			return new Event
			{
				Id = dto.Id,
				Title = dto.Title,
				Summary = dto.Summary,
				MinSpend = dto.MinSpend,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate,
				RewardDishId = dto.RewardDishId,
				DiscountType = dto.DiscountType,
				DiscountValue = dto.DiscountValue,
				Status = dto.Status,
				IsAutoDiscount = dto.IsAutoDiscount
			};
		}

		public static EventEditDto ToEditDto(this Event entity)
		{
			return new EventEditDto
			{
				Id = entity.Id,
				Title = entity.Title,
				Summary = entity.Summary,
				MinSpend = entity.MinSpend,
				StartDate = entity.StartDate,
				EndDate = (DateTime)entity.EndDate,
				RewardDishId = entity.RewardDishId,
				DiscountType = entity.DiscountType,
				DiscountValue = entity.DiscountValue,
				Status = entity.Status,
				IsAutoDiscount = entity.IsAutoDiscount
			};
		}


	}
}
