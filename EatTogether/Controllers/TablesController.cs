using EatTogether.Models.DTOs;
using EatTogether.Models.Infra;
using EatTogether.Models.Services;
using EatTogether.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
	[RequirePermission("Table_Manage")]
	public class TablesController : Controller
    {
        private readonly TableService         _tableService;
        private readonly ReservationService   _reservationService;
        private readonly WalkInQueueService   _walkInQueueService;

        public TablesController(
            TableService       tableService,
            ReservationService reservationService,
            WalkInQueueService walkInQueueService)
        {
            _tableService       = tableService;
            _reservationService = reservationService;
            _walkInQueueService = walkInQueueService;
        }

        // GET: /Tables
        public async Task<IActionResult> Index()
        {
            var dtos = await _tableService.GetAllAsync();

            // 今日待安排：訂位 Status=0 (訂位中) + 候位 Status=0/1 (等待中/已叫號)
            var today = DateTime.Today;
            var pendingReservations = await _reservationService.GetByDateAsync(today);
            var pendingWalkIns      = await _walkInQueueService.GetTodayPendingAsync();

            ViewBag.PendingReservations = pendingReservations
                .Where(r => r.Status == 0)
                .OrderBy(r => r.ReservationDate)
                .ToList();
            ViewBag.PendingWalkIns = pendingWalkIns;

            return View(dtos);
        }

        // GET: /Tables/Create
        public IActionResult Create()
        {
            return View(new TableCreateViewModel());
        }

        // POST: /Tables/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TableCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _tableService.CreateAsync(vm.ToDto());

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(nameof(vm.TableName), result.ErrorMessage);
                return View(vm);
            }

            TempData["SuccessMessage"] = $"桌位「{vm.TableName}」新增成功";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Tables/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _tableService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new TableEditViewModel
            {
                Id = dto.Id,
                TableName = dto.TableName,
                SeatCount = dto.SeatCount,
                Status = dto.Status,
                Remark = dto.Remark
            };
            return View(vm);
        }

        // POST: /Tables/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TableEditViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
                return View(vm);

            var result = await _tableService.UpdateAsync(new TableDto
            {
                Id = vm.Id,
                TableName = vm.TableName,
                SeatCount = vm.SeatCount,
                Remark = vm.Remark
            });

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(nameof(vm.TableName), result.ErrorMessage);
                return View(vm);
            }

            TempData["SuccessMessage"] = $"桌位「{vm.TableName}」修改成功";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Tables/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tableService.DeleteAsync(id);

            if (!result.IsSuccess)
                TempData["ErrorMessage"] = result.ErrorMessage;
            else
                TempData["SuccessMessage"] = "桌位已刪除";

            return RedirectToAction(nameof(Index));
        }

        // POST: /Tables/UpdateStatus  (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] TableUpdateStatusViewModel vm)
        {
            var result = await _tableService.UpdateStatusAsync(vm.Id, vm.Status);
            if (!result.IsSuccess)
                return Json(new { success = false, message = result.ErrorMessage ?? "" });

            // 設為保留(2)時儲存說明；空桌/用餐中時 Repository 已自動清除
            if (vm.Status == 2 && vm.Remark != null)
                await _tableService.UpdateRemarkAsync(vm.Id, vm.Remark);

            return Json(new { success = true, message = "" });
        }
    }
}
