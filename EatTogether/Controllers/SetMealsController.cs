using EatTogether.Models.DTOs;
using EatTogether.Models.Services;       
using EatTogether.Models.ViewModels;     
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EatTogether.Controllers        
{
    public class SetMealsController : Controller
    {
        private readonly SetMealService _setMealService;
        private readonly DishService _dishService;

        public SetMealsController(SetMealService setMealService, DishService dishService)
        {
            _setMealService = setMealService;
            _dishService    = dishService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _setMealService.GetAllAsync();
            var vms = dtos.Select(d => d.ToViewModel()).ToList();
            return View(vms);
        }

        public async Task<IActionResult> Create()
        {
            var allSetMeals = await _setMealService.GetAllAsync();
            int nextOrder = allSetMeals.Any() ? allSetMeals.Max(s => s.DisplayOrder) + 1 : 1;
            return View(new SetMealViewModel { DisplayOrder = nextOrder });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SetMealViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (!string.IsNullOrEmpty(vm.CroppedImageData))
                vm.ImageUrl = await SaveBase64ImageAsync(vm.CroppedImageData, vm.SetMealName);

            await _setMealService.CreateAsync(vm.ToDto());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _setMealService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto.ToViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] SetMealViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            if (!string.IsNullOrEmpty(vm.CroppedImageData))
            {
                // 強制覆蓋原有檔案，並用餐點名稱命名
                vm.ImageUrl = await SaveBase64ImageAsync(vm.CroppedImageData, vm.SetMealName);
            }

            await _setMealService.UpdateAsync(vm.ToDto());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            await _setMealService.DisableAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] SetMealItemViewModel vm)
        {
            try {
                await _setMealService.AddItemAsync(vm.ToItemDto());
                return Ok();
            } catch (InvalidOperationException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int id)
        {
            await _setMealService.RemoveItemAsync(id);
            return Ok();
        }

        private async Task<string> SaveBase64ImageAsync(string base64Data, string fileNamePrefix)
        {
            if (string.IsNullOrEmpty(base64Data)) return null;

            var base64 = base64Data.Contains(",") ? base64Data.Split(',')[1] : base64Data;
            var bytes = Convert.FromBase64String(base64);

            // 強制使用套餐名稱作為檔名
            string fileName = $"{fileNamePrefix}.jpg";
            foreach (char c in Path.GetInvalidFileNameChars()) {
                fileName = fileName.Replace(c, '_');
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var savePath = Path.Combine(folderPath, fileName);
            await System.IO.File.WriteAllBytesAsync(savePath, bytes);

            return "/images/" + fileName;
        }
    }
}
