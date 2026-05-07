using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using EatTogether.Models.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{

	public class EventRepository : IEventRepository
	{
		private readonly EatTogetherDBContext _context;

		public EventRepository(EatTogetherDBContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(EventCreateDto dto)
		{

			var events = dto.ToEntity();


			_context.Events.Add(events);
			await _context.SaveChangesAsync();
		}


		public async Task<List<EventDto>> GetAllAsync()
		{
			var data = await _context.Events
				.AsNoTracking()
                .Select(EventsMappingExtension.ToEventDtoExpression())
				.ToListAsync();

			return data;
		}


		public async Task EditAsync(EventEditDto dto)
		{
			var entity = await _context.Events.FindAsync(dto.Id);
			if (entity == null)
			{
				return;
			}

			entity.Title = dto.Title;
			entity.Summary = dto.Summary;
			entity.MinSpend = dto.MinSpend;
			entity.StartDate = dto.StartDate;
			entity.EndDate = dto.EndDate;
			entity.RewardDishId = dto.RewardDishId;
			entity.DiscountType = dto.DiscountType;
			entity.DiscountValue = dto.DiscountValue;
			entity.Status = dto.Status;
            entity.IsAutoDiscount = dto.IsAutoDiscount;

			await _context.SaveChangesAsync();		
		}

		public async Task<EventEditDto> GetEditByIdAsync(int id)
		{
			var entity = await _context.Events.FindAsync(id);

			if (entity == null) return null;

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

        public async Task<List<EventApplicableDto>> GetApplicableEventsAsync(int amount)
        {
            var today    = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var rows = await _context.Events
                .AsNoTracking()
                .Where(e => e.Status == 1
                         && e.IsAutoDiscount == 1
                         && e.StartDate < tomorrow
                         && (e.EndDate == null || e.EndDate >= today)
                         && e.MinSpend  <= amount)
                .OrderByDescending(e => e.MinSpend)
                .Select(e => new
                {
                    e.Id, e.Title, e.Summary, e.MinSpend,
                    e.DiscountType, e.DiscountValue, e.RewardDishId,
                    DishName = e.RewardDishId != null
                        ? _context.Dishes.Where(d => d.Id == e.RewardDishId).Select(d => d.DishName).FirstOrDefault()
                        : null
                })
                .ToListAsync();

            var result = new List<EventApplicableDto>();

            foreach (var e in rows)
            {
                int calculated = 0;
                string desc    = string.Empty;
                var dishName   = e.DishName ?? "";

                if (e.DiscountType == "FixedAmount")
                {
                    calculated = (int)e.DiscountValue;
                    desc = $"折抵 NT${calculated}";
                }
                else if (e.DiscountType == "Percent")
                {
                    calculated = (int)Math.Round(amount * (1 - (double)e.DiscountValue / 10));
                    desc = $"打 {e.DiscountValue} 折，省 NT${calculated}";
                }
                else
                {
                    desc = $"贈送：{dishName}";
                }

                result.Add(new EventApplicableDto
                {
                    Id                  = e.Id,
                    Title               = e.Title,
                    Summary             = e.Summary ?? string.Empty,
                    DiscountType        = e.DiscountType,
                    DiscountValue       = e.DiscountValue,
                    RewardDishId        = e.RewardDishId,
                    RewardDishName      = string.IsNullOrEmpty(dishName) ? null : dishName,
                    MinSpend            = e.MinSpend,
                    CalculatedDiscount  = calculated,
                    DiscountDescription = desc
                });
            }

            return result;
        }

        public async Task<List<EventApplicableDto>> GetManualEventsAsync(int amount)
        {
            var today = DateTime.Today;
            var rows = await _context.Events
                .AsNoTracking()
                .Where(e => e.StartDate <= today
                         && (e.EndDate == null || e.EndDate >= today))
                .OrderByDescending(e => e.MinSpend)
                .Select(e => new
                {
                    e.Id, e.Title, e.Summary, e.MinSpend,
                    e.DiscountType, e.DiscountValue,
                    e.RewardDishId,
                    DishName = e.RewardDishId != null
                        ? _context.Dishes.Where(d => d.Id == e.RewardDishId).Select(d => d.DishName).FirstOrDefault()
                        : null
                })
                .ToListAsync();

            var result = new List<EventApplicableDto>();
            foreach (var e in rows)
            {
                bool eligible = e.MinSpend <= amount;
                int calculated = 0;
                string desc = string.Empty;
                var dishName = e.DishName ?? "";

                if (e.DiscountType == "FixedAmount")
                {
                    calculated = (int)e.DiscountValue;
                    desc = $"折抵 NT${calculated}";
                }
                else if (e.DiscountType == "Percent")
                {
                    calculated = eligible
                        ? (int)(amount * e.DiscountValue / 100m)
                        : 0;
                    // ↓ 不管是否符合都給描述
                    desc = eligible
                        ? $"折扣 {e.DiscountValue}%，省 NT${calculated}"
                        : $"折扣 {e.DiscountValue}%";
                }
                else
                {
                    desc = $"贈送：{dishName}";
                }

                result.Add(new EventApplicableDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Summary = e.Summary ?? string.Empty,
                    DiscountType = e.DiscountType,
                    DiscountValue = e.DiscountValue,
                    RewardDishId = e.RewardDishId,
                    RewardDishName = string.IsNullOrEmpty(dishName) ? null : dishName,
                    MinSpend = e.MinSpend,
                    CalculatedDiscount = calculated,
                    DiscountDescription = desc,
                    IsEligible = eligible
                });
            }
            return result;
        }

        public async Task<List<EventApplicableDto>> GetEventsByIdsAsync(IEnumerable<int> ids)
        {
            var idSet = ids.ToHashSet();
            if (idSet.Count == 0) return new List<EventApplicableDto>();

            var rows = await _context.Events
                .AsNoTracking()
                .Where(e => idSet.Contains(e.Id))
                .Select(e => new
                {
                    e.Id, e.Title, e.Summary, e.MinSpend,
                    e.DiscountType, e.DiscountValue, e.RewardDishId,
                    DishName = e.RewardDishId != null
                        ? _context.Dishes.Where(d => d.Id == e.RewardDishId).Select(d => d.DishName).FirstOrDefault()
                        : null
                })
                .ToListAsync();

            var result = new List<EventApplicableDto>();
            foreach (var e in rows)
            {
                var dishName = e.DishName ?? "";
                string desc;
                int calculated = 0;

                if (e.DiscountType == "FixedAmount")
                {
                    calculated = (int)e.DiscountValue;
                    desc = $"折抵 NT${calculated}";
                }
                else if (e.DiscountType == "Percent")
                {
                    desc = $"折扣 {e.DiscountValue}%";
                }
                else
                {
                    desc = $"贈送：{dishName}";
                }

                result.Add(new EventApplicableDto
                {
                    Id                  = e.Id,
                    Title               = e.Title,
                    Summary             = e.Summary ?? string.Empty,
                    DiscountType        = e.DiscountType,
                    DiscountValue       = e.DiscountValue,
                    RewardDishId        = e.RewardDishId,
                    RewardDishName      = string.IsNullOrEmpty(dishName) ? null : dishName,
                    MinSpend            = e.MinSpend,
                    CalculatedDiscount  = calculated,
                    DiscountDescription = desc
                });
            }
            return result;
        }

        public async Task<(string DiscountType, string? RewardDishName)?> GetEventGiftInfoAsync(int eventId)
        {
            var ev = await _context.Events
                .AsNoTracking()
                .Include(e => e.RewardDish)
                .Where(e => e.Id == eventId)
                .Select(e => new { e.DiscountType, RewardDishName = e.RewardDish != null ? e.RewardDish.DishName : null })
                .FirstOrDefaultAsync();

            if (ev == null) return null;
            return (ev.DiscountType, ev.RewardDishName);
        }
    }
}
