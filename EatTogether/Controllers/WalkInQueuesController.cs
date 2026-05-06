using EatTogether.Models.DTOs;
using EatTogether.Models.Infra;
using EatTogether.Models.Repositories;
using EatTogether.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
    [RequirePermission("Table_Manage")]
    public class WalkInQueuesController : Controller
    {
        private readonly WalkInQueueService _service;
        private readonly ITableRepository   _tableRepo;

        public WalkInQueuesController(WalkInQueueService service, ITableRepository tableRepo)
        {
            _service   = service;
            _tableRepo = tableRepo;
        }

        // GET: /WalkInQueues
        public async Task<IActionResult> Index()
        {
            var list   = await _service.GetTodayAsync();
            var tables = await _tableRepo.GetAllAsync();
            ViewBag.Tables = tables;
            return View(list);
        }

        // POST: /WalkInQueues/Register  (AJAX)
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] WalkInQueueDto dto)
        {
            var result = await _service.RegisterAsync(dto);
            if (!result.IsSuccess)
                return Json(new { success = false, message = result.ErrorMessage });

            return Json(new { success = true, id = result.Value });
        }

        // POST: /WalkInQueues/Call/5  (AJAX)
        [HttpPost]
        public async Task<IActionResult> Call(int id)
        {
            var result = await _service.CallAsync(id);
            if (!result.IsSuccess)
                return Json(new { success = false, message = result.ErrorMessage });

            return Json(new { success = true });
        }

        // POST: /WalkInQueues/Seat  (AJAX)
        [HttpPost]
        public async Task<IActionResult> Seat([FromBody] SeatViewModel vm)
        {
            var result = await _service.SeatAsync(vm.Id, vm.TableId);
            if (!result.IsSuccess)
                return Json(new { success = false, message = result.ErrorMessage });

            return Json(new { success = true });
        }

        // POST: /WalkInQueues/NoShow/5  (AJAX)
        [HttpPost]
        public async Task<IActionResult> NoShow(int id)
        {
            var result = await _service.NoShowAsync(id);
            if (!result.IsSuccess)
                return Json(new { success = false, message = result.ErrorMessage });

            return Json(new { success = true });
        }

        // POST: /WalkInQueues/Leave/5  (AJAX)
        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var result = await _service.LeaveAsync(id);
            if (!result.IsSuccess)
                return Json(new { success = false, message = result.ErrorMessage });

            return Json(new { success = true });
        }

        public class SeatViewModel
        {
            public int Id      { get; set; }
            public int TableId { get; set; }
        }
    }
}
