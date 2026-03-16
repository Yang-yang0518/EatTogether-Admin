using EatTogether.Models.DTOs;

namespace EatTogether.Models.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
    }
}
