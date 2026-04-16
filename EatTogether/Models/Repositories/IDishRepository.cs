using EatTogether.Models.DTOs;

namespace EatTogether.Models.Repositories
{
    public interface IDishRepository
    {
        Task<IEnumerable<DishDto>> GetAllAsync();
        Task<IEnumerable<DishDto>> GetAllActiveAsync();
        Task<DishDto?> GetByIdAsync(int id);
        Task CreateAsync(DishDto dto);
        Task UpdateAsync(DishDto dto);
        Task SoftDeleteAsync(int id);
        Task BatchSoftDeleteAsync(IEnumerable<int> ids);
        Task EnableAsync(int id);
        Task BatchEnableAsync(IEnumerable<int> ids);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(IEnumerable<int> ids);
        Task UpdateOrderAsync(IEnumerable<int> orderedIds);
        Task<bool> UpdateStockAsync(int id, int stockStatus);
        Task<(double averageScore, int ratingCount)?> RateAsync(int id, int score);
    }
}
