using EatTogether.Models.DTOs;

namespace EatTogether.Models.Repositories
{
    public interface IWalkInQueueRepository
    {
        Task<IEnumerable<WalkInQueueDto>> GetTodayAsync();
        Task<WalkInQueueDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(WalkInQueueDto dto);
        Task UpdateStatusAsync(int id, int newStatus, DateTime? timestamp, int? tableId = null);
        Task<string> GetNextQueueNumberAsync(DateTime date);
        Task<IEnumerable<WalkInQueueDto>> GetTodayPendingAsync();
    }
}
