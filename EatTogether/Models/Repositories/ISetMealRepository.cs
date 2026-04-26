using EatTogether.Models.DTOs;

namespace EatTogether.Models.Repositories
{
	public interface ISetMealRepository
	{
		Task<IEnumerable<Setmealdto>> GetAllAsync();
		Task<IEnumerable<Setmealdto>> GetAllActiveAsync();
		Task<Setmealdto?> GetByIdAsync(int id);
		Task CreateAsync(Setmealdto dto);
		Task UpdateAsync(Setmealdto dto);
		Task SoftDeleteAsync(int id);
		Task BatchSoftDeleteAsync(IEnumerable<int> ids);
		Task EnableAsync(int id);
		Task BatchEnableAsync(IEnumerable<int> ids);
		Task DeleteAsync(int id);
		Task BatchDeleteAsync(IEnumerable<int> ids);
		Task AddItemAsync(SetmealItemDto itemDto);
		Task RemoveItemAsync(int itemId);
		Task UpdateItemsAsync(int setMealId, IEnumerable<SetmealItemDto> itemDtos);
		Task UpdateOrderAsync(IEnumerable<int> orderedIds);
		Task<int> CloneSetMealAsync(int id);
	}
}
