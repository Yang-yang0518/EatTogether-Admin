using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using EatTogether.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{
    public class WalkInQueueRepository : IWalkInQueueRepository
    {
        private readonly EatTogetherDBContext _context;

        public WalkInQueueRepository(EatTogetherDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WalkInQueueDto>> GetTodayAsync()
        {
            var today = DateTime.Today;
            return await _context.WalkInQueues
                .Where(q => q.RegisteredAt.Date == today)
                .OrderBy(q => q.RegisteredAt)
                .Select(q => new WalkInQueueDto
                {
                    Id            = q.Id,
                    QueueNumber   = q.QueueNumber,
                    Name          = q.Name,
                    Phone         = q.Phone,
                    AdultsCount   = q.AdultsCount,
                    ChildrenCount = q.ChildrenCount,
                    Status        = q.Status,
                    Remark        = q.Remark,
                    RegisteredAt  = q.RegisteredAt,
                    CalledAt      = q.CalledAt,
                    SeatedAt      = q.SeatedAt,
                    LeftAt        = q.LeftAt,
                    MemberId      = q.MemberId,
                    TableId       = q.TableId,
                    TableName     = q.Table != null ? q.Table.TableName : null
                })
                .ToListAsync();
        }

        public async Task<WalkInQueueDto?> GetByIdAsync(int id)
        {
            var q = await _context.WalkInQueues
                .Include(x => x.Table)
                .FirstOrDefaultAsync(x => x.Id == id);

            return q?.ToDto(q.Table?.TableName);
        }

        public async Task<int> CreateAsync(WalkInQueueDto dto)
        {
            var entity = new WalkInQueue
            {
                QueueNumber   = dto.QueueNumber,
                Name          = dto.Name,
                Phone         = dto.Phone,
                AdultsCount   = dto.AdultsCount,
                ChildrenCount = dto.ChildrenCount,
                Status        = 0,
                Remark        = dto.Remark,
                RegisteredAt  = DateTime.Now,
                MemberId      = dto.MemberId
            };
            _context.WalkInQueues.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateStatusAsync(int id, int newStatus, DateTime? timestamp, int? tableId = null)
        {
            var entity = await _context.WalkInQueues.FindAsync(id);
            if (entity == null) return;

            entity.Status = newStatus;

            switch (newStatus)
            {
                case 1: // 已叫號
                    entity.CalledAt = timestamp ?? DateTime.Now;
                    break;
                case 2: // 已入座
                    entity.SeatedAt = timestamp ?? DateTime.Now;
                    if (tableId.HasValue)
                        entity.TableId = tableId;
                    break;
                case 3: // 已離開
                case 4: // 已過號
                    entity.LeftAt = timestamp ?? DateTime.Now;
                    break;
            }

            await _context.SaveChangesAsync();
        }

        /// <summary>產生當天下一個叫號號碼，格式 A001、A002 …</summary>
        public async Task<string> GetNextQueueNumberAsync(DateTime date)
        {
            var last = await _context.WalkInQueues
                .Where(q => q.RegisteredAt.Date == date.Date)
                .OrderByDescending(q => q.Id)
                .Select(q => q.QueueNumber)
                .FirstOrDefaultAsync();

            if (last == null) return "A001";

            // 取字母前綴和數字部分
            var prefix = new string(last.TakeWhile(char.IsLetter).ToArray());
            var numStr = new string(last.SkipWhile(char.IsLetter).ToArray());

            if (int.TryParse(numStr, out int num))
                return $"{prefix}{(num + 1):D3}";

            return "A001";
        }

        /// <summary>今日尚未安排（Status=0 等待中, Status=1 已叫號）的候位</summary>
        public async Task<IEnumerable<WalkInQueueDto>> GetTodayPendingAsync()
        {
            var today = DateTime.Today;
            return await _context.WalkInQueues
                .Where(q => q.RegisteredAt.Date == today && (q.Status == 0 || q.Status == 1))
                .OrderBy(q => q.RegisteredAt)
                .Select(q => new WalkInQueueDto
                {
                    Id            = q.Id,
                    QueueNumber   = q.QueueNumber,
                    Name          = q.Name,
                    Phone         = q.Phone,
                    AdultsCount   = q.AdultsCount,
                    ChildrenCount = q.ChildrenCount,
                    Status        = q.Status,
                    Remark        = q.Remark,
                    RegisteredAt  = q.RegisteredAt,
                    CalledAt      = q.CalledAt,
                    MemberId      = q.MemberId,
                    TableId       = q.TableId
                })
                .ToListAsync();
        }
    }
}
