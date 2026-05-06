using EatTogether.Models.DTOs;
using EatTogether.Models.Infra;
using EatTogether.Models.Repositories;

namespace EatTogether.Models.Services
{
    public class WalkInQueueService
    {
        private readonly IWalkInQueueRepository _repo;
        private readonly ITableRepository       _tableRepo;

        public WalkInQueueService(IWalkInQueueRepository repo, ITableRepository tableRepo)
        {
            _repo      = repo;
            _tableRepo = tableRepo;
        }

        public async Task<IEnumerable<WalkInQueueDto>> GetTodayAsync()
            => await _repo.GetTodayAsync();

        public async Task<WalkInQueueDto?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<WalkInQueueDto>> GetTodayPendingAsync()
            => await _repo.GetTodayPendingAsync();

        /// <summary>現場登記候位</summary>
        public async Task<Result<int>> RegisterAsync(WalkInQueueDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result<int>.Fail("姓名不可為空");
            if (string.IsNullOrWhiteSpace(dto.Phone))
                return Result<int>.Fail("電話不可為空");
            if (dto.AdultsCount <= 0)
                return Result<int>.Fail("大人數至少 1 人");

            dto.QueueNumber = await _repo.GetNextQueueNumberAsync(DateTime.Today);
            var id = await _repo.CreateAsync(dto);
            return Result<int>.Success(id);
        }

        /// <summary>叫號（Status 0 → 1）</summary>
        public async Task<Result> CallAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return Result.Fail("找不到此候位記錄");
            if (q.Status != 0) return Result.Fail("僅等待中的候位才能叫號");

            await _repo.UpdateStatusAsync(id, 1, DateTime.Now);
            return Result.Success();
        }

        /// <summary>入座（Status 1 → 2，指定桌位，同時把桌位調為用餐中）</summary>
        public async Task<Result> SeatAsync(int id, int tableId)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return Result.Fail("找不到此候位記錄");
            if (q.Status != 1) return Result.Fail("僅已叫號的候位才能入座");

            var table = await _tableRepo.GetByIdAsync(tableId);
            if (table == null) return Result.Fail("找不到此桌位");
            if (table.Status == 1) return Result.Fail($"「{table.TableName}」目前用餐中，請選擇其他桌位");

            // 候位入座
            await _repo.UpdateStatusAsync(id, 2, DateTime.Now, tableId);
            // 桌位 → 用餐中
            await _tableRepo.UpdateStatusAsync(tableId, 1);
            return Result.Success();
        }

        /// <summary>過號（Status 1 → 4）</summary>
        public async Task<Result> NoShowAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return Result.Fail("找不到此候位記錄");
            if (q.Status != 1) return Result.Fail("僅已叫號的候位才能標記過號");

            await _repo.UpdateStatusAsync(id, 4, DateTime.Now);
            return Result.Success();
        }

        /// <summary>離開/放棄（Status 0/1 → 3）</summary>
        public async Task<Result> LeaveAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return Result.Fail("找不到此候位記錄");
            if (q.Status != 0 && q.Status != 1)
                return Result.Fail("僅等待中或已叫號的候位才能標記離開");

            await _repo.UpdateStatusAsync(id, 3, DateTime.Now);
            return Result.Success();
        }
    }
}
