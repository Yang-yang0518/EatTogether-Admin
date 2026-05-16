using EatTogether.Models.DTOs;

namespace EatTogether.Models.Repositories
{
	public interface ISetMealRepository
	{
		Task<IEnumerable<SetMealDto>> GetAllAsync();
		Task<IEnumerable<SetMealDto>> GetAllActiveAsync();
		Task<SetMealDto?> GetByIdAsync(int id);
		Task CreateAsync(SetMealDto dto);
		Task UpdateAsync(SetMealDto dto);
		Task SoftDeleteAsync(int id);
		Task BatchSoftDeleteAsync(IEnumerable<int> ids);
		Task EnableAsync(int id);
		Task BatchEnableAsync(IEnumerable<int> ids);
		Task DeleteAsync(int id);
		Task BatchDeleteAsync(IEnumerable<int> ids);
		Task AddItemAsync(SetMealItemDto itemDto);
		Task RemoveItemAsync(int itemId);
		Task UpdateItemsAsync(int setMealId, IEnumerable<SetMealItemDto> itemDtos);
		Task UpdateOrderAsync(IEnumerable<int> orderedIds);
		Task<int> CloneSetMealAsync(int id);
	}
}
