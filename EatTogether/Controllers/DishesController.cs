using EatTogether.Models.Services;
using EatTogether.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EatTogether.Controllers
{
    public class DishesController : Controller
    {
        private readonly DishService _dishService;
        private readonly CategoryService _categoryService;

        public DishesController(DishService dishService, CategoryService categoryService)
        {
            _dishService     = dishService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _dishService.GetAllAsync();
            var vms = dtos.Select(d => {
                var vm = d.ToViewModel();
                if (string.IsNullOrEmpty(vm.ImageUrl))
                {
                    // Sanitize dish name for filename comparison
                    string safeDishName = vm.DishName;
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        safeDishName = safeDishName.Replace(c, '_');
                    }

                    // Determine the base path for wwwroot/images
                    var baseImagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Check for .jpg file
                    string jpgFileName = $"{safeDishName}.jpg";
                    string jpgPath = Path.Combine(baseImagesFolderPath, jpgFileName);
                    if (System.IO.File.Exists(jpgPath))
                    {
                        vm.ImageUrl = "/images/" + jpgFileName;
                    }
                    else
                    {
                        // Check for .png file if .jpg is not found
                        string pngFileName = $"{safeDishName}.png";
                        string pngPath = Path.Combine(baseImagesFolderPath, pngFileName);
                        if (System.IO.File.Exists(pngPath))
                        {
                            vm.ImageUrl = "/images/" + pngFileName;
                        }
                    }
                }
                return vm;
            }).ToList();
            return View(vms);
        }

        public async Task<IActionResult> Create()
        {
            var allDishes = await _dishService.GetAllAsync();
            int nextOrder = allDishes.Any() ? allDishes.Max(d => d.DisplayOrder) + 1 : 1;
            var vm = new DishViewModel { DisplayOrder = nextOrder };
            vm.CategoryOptions = await GetCategoryOptionsAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] DishViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.CategoryOptions = await GetCategoryOptionsAsync();
                return View(vm);
            }

            if (!string.IsNullOrEmpty(vm.CroppedImageData))
            {
                // 新增時，直接用餐點名稱命名
                vm.ImageUrl = await SaveBase64ImageAsync(vm.CroppedImageData, vm.DishName);
            }

            await _dishService.CreateAsync(vm.ToDto());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _dishService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            var vm = dto.ToViewModel();
            vm.CategoryOptions = await GetCategoryOptionsAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] DishViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.CategoryOptions = await GetCategoryOptionsAsync();
                return View(vm);
            }

            if (!string.IsNullOrEmpty(vm.CroppedImageData))
            {
                // 【核心修改】：編輯時，直接拿「目前的餐點名稱」去覆蓋檔案
                vm.ImageUrl = await SaveBase64ImageAsync(vm.CroppedImageData, vm.DishName);
            }

            await _dishService.UpdateAsync(vm.ToDto());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            await _dishService.DisableAsync(id);
            return Ok();
        }

        public async Task<IActionResult> GetAllJson()
        {
            var dtos = await _dishService.GetAllAsync();
            return Json(dtos.Select(d => new { id = d.Id, dishName = d.DishName, price = d.Price }));     
        }

        private async Task<List<SelectListItem>> GetCategoryOptionsAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            return categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName }).ToList();
        }

        /// <summary>
        /// 儲存圖片並強制使用餐點名稱命名（達成覆蓋效果）
        /// </summary>
        private async Task<string> SaveBase64ImageAsync(string base64Data, string dishName)
        {
            if (string.IsNullOrEmpty(base64Data)) return null;

            var base64 = base64Data.Contains(",") ? base64Data.Split(',')[1] : base64Data;
            var bytes = Convert.FromBase64String(base64);

            // 【強制規範】：檔名 = 餐點名稱.jpg
            // 這樣不論改幾次，只要餐點名稱不變，檔案就會被 WriteAllBytesAsync 強制覆蓋
            string fileName = $"{dishName}.jpg";
            
            // 移除檔名中可能導致報錯的特殊字元
            foreach (char c in Path.GetInvalidFileNameChars()) {
                fileName = fileName.Replace(c, '_');
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var savePath = Path.Combine(folderPath, fileName);
            
            // 執行寫入（若檔案已存在，System.IO 會直接覆蓋它）
            await System.IO.File.WriteAllBytesAsync(savePath, bytes);

            return "/images/" + fileName;
        }
    }
}
