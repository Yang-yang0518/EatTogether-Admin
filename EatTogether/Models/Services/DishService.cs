using EatTogether.Models.DTOs;
using EatTogether.Models.Repositories;

namespace EatTogether.Models.Services
{
	public class DishService
	{
		private readonly IDishRepository _repo;

		public DishService(IDishRepository repo)
		{
			_repo = repo;
		}
		
		public async Task<IEnumerable<DishDto>> GetAllAsync()
		{
			return await _repo.GetAllAsync();
		}

		public async Task<IEnumerable<DishDto>> GetAllActiveAsync()
		{
			return await _repo.GetAllActiveAsync();
		}

		public async Task<DishDto?> GetByIdAsync(int id)
		{
			return await _repo.GetByIdAsync(id);
		}

		public async Task CreateAsync(DishDto dto)
		{
			await _repo.CreateAsync(dto);
		}

		public async Task UpdateAsync(DishDto dto)
		{
			await _repo.UpdateAsync(dto);
		}

		public async Task SoftDeleteAsync(int id)
		{
			await _repo.SoftDeleteAsync(id);
		}

		public async Task DisableAsync(int id)
		{
			await _repo.SoftDeleteAsync(id);
		}

		public async Task BatchDisableAsync(IEnumerable<int> ids)
		{
			await _repo.BatchSoftDeleteAsync(ids);
		}

		public async Task EnableAsync(int id)
		{
			await _repo.EnableAsync(id);
		}

		public async Task BatchEnableAsync(IEnumerable<int> ids)
		{
			await _repo.BatchEnableAsync(ids);
		}

		public async Task DeleteAsync(int id)
		{
			await _repo.DeleteAsync(id);
		}

		public async Task BatchDeleteAsync(IEnumerable<int> ids)
		{
			await _repo.BatchDeleteAsync(ids);
		}

		public async Task UpdateOrderAsync(IEnumerable<int> orderedIds)
		{
			await _repo.UpdateOrderAsync(orderedIds);
		}

		public async Task<bool> UpdateStockAsync(int id, int stockStatus)
		{
			return await _repo.UpdateStockAsync(id, stockStatus);
		}

		public async Task<(double averageScore, int ratingCount)?> RateAsync(int id, int score)
		{
			return await _repo.RateAsync(id, score);
		}
	}
}
