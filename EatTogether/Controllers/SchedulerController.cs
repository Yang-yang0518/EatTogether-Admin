using EatTogether.Models.EfModels;
using EatTogether.Models.Infra;
using EatTogether.Models.Services;
using EatTogether.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Controllers
{
    [RequirePermission("Menu_Manage")]
    public class SchedulerController : Controller
    {
        private readonly EatTogetherDBContext _db;
        private readonly DishSchedulerService _scheduler;

        public SchedulerController(EatTogetherDBContext db, DishSchedulerService scheduler)
        {
            _db = db;
            _scheduler = scheduler;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _db.SchedulerLogs
                .OrderByDescending(l => l.ExecutedAt)
                .Take(50)
                .ToListAsync();
            return View(logs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var log = await _db.SchedulerLogs.FindAsync(id);
            if (log == null) return NotFound();

            var detail = log.DetailJson != null
                ? System.Text.Json.JsonSerializer.Deserialize<SchedulerDetailJson>(log.DetailJson)
                : new SchedulerDetailJson();

            var vm = new SchedulerDetailViewModel
            {
                Log = log,
                DishesEnabled = detail.enabledDishes ?? new List<string>(),
                DishesDisabled = detail.disabledDishes ?? new List<string>(),
                MealsEnabled = detail.enabledMeals ?? new List<string>(),
                MealsDisabled = detail.disabledMeals ?? new List<string>()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> TriggerNow()
        {
            await _scheduler.RunAsync("手動");
            return RedirectToAction("Index");
        }
    }
}
