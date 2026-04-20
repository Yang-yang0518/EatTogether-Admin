using EatTogether.Models.EfModels;
using EatTogether.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using EfSchedulerLog = EatTogether.Models.EfModels.SchedulerLog;

namespace EatTogether.Models.Services
{
    public class DishSchedulerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DishSchedulerService> _logger;

        public DishSchedulerService(IServiceScopeFactory scopeFactory, ILogger<DishSchedulerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try { await RunAsync("自動"); }
            catch (Exception ex) { _logger.LogError(ex, "DishSchedulerService 執行失敗"); }

            while (!stoppingToken.IsCancellationRequested)
            {
                var taiwanZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                var nowTaiwan = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taiwanZone);
                var nextRun = nowTaiwan.Date.AddDays(1);
                var delay = nextRun - nowTaiwan;
                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested) break;

                try { await RunAsync("自動"); }
                catch (Exception ex) { _logger.LogError(ex, "DishSchedulerService 執行失敗"); }
            }
        }

        public async Task RunAsync(string triggerType)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EatTogetherDBContext>();
            var today = DateOnly.FromDateTime(DateTime.Today);

            var dishes = await db.Dishes
                .Where(d => d.StartDate != null || d.EndDate != null)
                .ToListAsync();

            var disabledDishes = new List<Dish>();
            var enabledDishes = new List<Dish>();
            foreach (var dish in dishes)
            {
                if (dish.EndDate != null && today >= dish.EndDate && dish.IsActive)
                {
                    dish.IsActive = false;
                    dish.UpdatedAt = DateTime.UtcNow;
                    disabledDishes.Add(dish);
                }
                else if (dish.StartDate != null && dish.EndDate != null
                    && today >= dish.StartDate && today <= dish.EndDate
                    && !dish.IsActive)
                {
                    dish.IsActive = true;
                    dish.UpdatedAt = DateTime.UtcNow;
                    enabledDishes.Add(dish);
                }
            }

            var setMeals = await db.SetMeals
                .Where(s => s.StartDate != null || s.EndDate != null)
                .ToListAsync();

            var disabledMeals = new List<SetMeal>();
            var enabledMeals = new List<SetMeal>();
            foreach (var meal in setMeals)
            {
                if (meal.EndDate != null && today >= meal.EndDate && meal.IsActive)
                {
                    meal.IsActive = false;
                    meal.UpdatedAt = DateTime.UtcNow;
                    disabledMeals.Add(meal);
                }
                else if (meal.StartDate != null && meal.EndDate != null
                    && today >= meal.StartDate && today <= meal.EndDate
                    && !meal.IsActive)
                {
                    meal.IsActive = true;
                    meal.UpdatedAt = DateTime.UtcNow;
                    enabledMeals.Add(meal);
                }
            }

            var detail = new
            {
                disabledDishes = disabledDishes.Select(d => d.DishName).ToList(),
                enabledDishes  = enabledDishes.Select(d => d.DishName).ToList(),
                disabledMeals  = disabledMeals.Select(m => m.SetMealName).ToList(),
                enabledMeals   = enabledMeals.Select(m => m.SetMealName).ToList()
            };

            db.SchedulerLogs.Add(new EfSchedulerLog
            {
                ExecutedAt = DateTime.Now,
                DishesEnabled = enabledDishes.Count,
                DishesDisabled = disabledDishes.Count,
                MealsEnabled = enabledMeals.Count,
                MealsDisabled = disabledMeals.Count,
                TriggerType = triggerType,
                DetailJson = System.Text.Json.JsonSerializer.Serialize(detail)
            });

            await db.SaveChangesAsync();
            _logger.LogInformation("DishSchedulerService 完成排程（{Type}），執行時間：{Time}", triggerType, DateTime.Now);
        }
    }
}
