using EatTogether.Models.DTOs;
using EatTogether.Models.Infra;
using EatTogether.Models.Services;
using EatTogether.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EatTogether.Controllers
{
	[Route("[controller]")]
	public class DishesController : Controller
    {
        private readonly DishService _dishService;
        private readonly CategoryService _categoryService;

        public DishesController(DishService dishService, CategoryService categoryService)
        {
            _dishService     = dishService;
            _categoryService = categoryService;
        }

		[HttpGet("Index")]
		public async Task<IActionResult> Index(bool newDish = false)
		{
            var dtos = await _dishService.GetAllAsync();
            var vms = dtos.Select(d => {
                var vm = d.ToViewModel();
                vm.ImageUrl = ResolveImageUrl(vm.ImageUrl, vm.DishName);
                return vm;
            }).ToList();
            return View(vms);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var allDishes = await _dishService.GetAllAsync();
            int nextOrder = allDishes.Any() ? allDishes.Max(d => d.DisplayOrder) + 1 : 1;
            var vm = new DishViewModel { DisplayOrder = nextOrder };
            vm.CategoryOptions = await GetCategoryOptionsAsync();
            return View(vm);
        }

        [HttpPost("Create")]
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
			return RedirectToAction(nameof(Index), new { newDish = true });
		}

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _dishService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            var vm = dto.ToViewModel();
            vm.CategoryOptions = await GetCategoryOptionsAsync();

			vm.ImageUrl = ResolveImageUrl(vm.ImageUrl, vm.DishName);
			return View(vm);
        }

        [HttpPost("Edit/{id}")]
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

        [HttpPost("Disable/{id}")]
        public async Task<IActionResult> Disable(int id)
        {
            await _dishService.DisableAsync(id);
            return Ok();
        }

        [HttpPost("BatchDisable")]
        public async Task<IActionResult> BatchDisable([FromBody] BatchRequestDto request)
        {
            if (request?.Ids == null || !request.Ids.Any()) return BadRequest("無項目可操作。");
            await _dishService.BatchDisableAsync(request.Ids);
            return Ok();
        }

        [HttpPost("Enable/{id}")]
        public async Task<IActionResult> Enable(int id)
        {
            await _dishService.EnableAsync(id);
            return Ok();
        }

        [HttpPost("BatchEnable")]
        public async Task<IActionResult> BatchEnable([FromBody] BatchRequestDto request)
        {
            if (request?.Ids == null || !request.Ids.Any()) return BadRequest("無項目可操作。");
            await _dishService.BatchEnableAsync(request.Ids);
            return Ok();
        }

        [HttpPost("BatchDelete")]
        public async Task<IActionResult> BatchDelete([FromBody] BatchRequestDto request)
        {
            if (request?.Ids == null || !request.Ids.Any()) return BadRequest("無項目可操作。");
            await _dishService.BatchDeleteAsync(request.Ids);
            return Ok();
        }

        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderedIdsDto request)
        {
            if (request?.OrderedIds == null || !request.OrderedIds.Any()) return BadRequest("無順序可更新。");
            await _dishService.UpdateOrderAsync(request.OrderedIds);
            return Ok();
        }

        [HttpPost("ToggleActive/{id}")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null) return NotFound();
            dish.IsActive = !dish.IsActive;
            await _dishService.UpdateAsync(dish);
            return Ok(new { isActive = dish.IsActive });
        }

		[HttpGet("GetAllJson")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllJson()
        {
            var dtos = await _dishService.GetAllAsync();
            return Json(dtos.Select(d => new
            {
                id = d.Id,
                dishName = d.DishName,
                price = d.Price,
                categoryId = d.CategoryId, 
                imageUrl = d.ImageUrl       //補上圖片路徑，前台才好顯示}));     
            }));
        }
		[HttpGet("GetActiveJson")]
		[HttpGet("active")]
		[AllowAnonymous]
		public async Task<IActionResult> GetActiveJson()
		{
			var dtos = await _dishService.GetAllActiveAsync();

			return Json(dtos.Select(d => {
				string imageUrl = ResolveImageUrl(d.ImageUrl, d.DishName);

				return new
				{
					id = d.Id,
					dishName = d.DishName,
					description = d.Description,
					price = d.Price,
					categoryId = d.CategoryId,
					categoryName = d.CategoryName,
					imageUrl = imageUrl,  // ← 用補過的
					isRecommended = d.IsRecommended,
					isPopular = d.IsPopular,
					isVegetarian = d.IsVegetarian,
					spicyLevel = d.SpicyLevel,
					ingredientsJson = d.IngredientsJson,
					isLimited = d.IsLimited,
					startDate = d.StartDate,
					endDate = d.EndDate,
					averageScore = d.AverageScore,
					ratingCount = d.RatingCount,
					stockStatus = d.StockStatus
				};
			}));
		}

		[HttpGet("GetByIdJson")]
		[AllowAnonymous]
		public async Task<IActionResult> GetByIdJson(int id)
		{
			var dto = await _dishService.GetByIdAsync(id);
			if (dto == null) return NotFound();
			return Json(new
			{
				id = dto.Id,
				dishName = dto.DishName,
				description = dto.Description,
				price = dto.Price,
				categoryId = dto.CategoryId,
				categoryName = dto.CategoryName,
				imageUrl = dto.ImageUrl,
				isRecommended = dto.IsRecommended,
				isPopular = dto.IsPopular,
				isVegetarian = dto.IsVegetarian,
				spicyLevel = dto.SpicyLevel
			});
		}


		private string ResolveImageUrl(string storedUrl, string name)
		{
			if (!string.IsNullOrEmpty(storedUrl)) return storedUrl;

			string safeName = name;
			foreach (char c in Path.GetInvalidFileNameChars())
				safeName = safeName.Replace(c, '_');

			var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
			if (System.IO.File.Exists(Path.Combine(folder, safeName + ".jpg")))
				return "/images/" + safeName + ".jpg";
			if (System.IO.File.Exists(Path.Combine(folder, safeName + ".png")))
				return "/images/" + safeName + ".png";

			return storedUrl;
		}

		private async Task<List<SelectListItem>> GetCategoryOptionsAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            return categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName }).ToList();
        }

        /// <summary>
        /// 儲存裁切後的圖片。存檔前會先刪除同名的 .png 和 .jpeg 檔案，確保只保留最新的 .jpg。
        /// </summary>
        private async Task<string> SaveBase64ImageAsync(string base64Data, string dishName)
        {
            if (string.IsNullOrEmpty(base64Data)) return null;

            var base64 = base64Data.Contains(",") ? base64Data.Split(',')[1] : base64Data;
            var bytes = Convert.FromBase64String(base64);

            // 移除檔名中不合法的字元
            string fileNamePrefix = dishName;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileNamePrefix = fileNamePrefix.Replace(c, '_');
            }

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 刪除舊的 .png 和 .jpeg 檔案
            string pngToDelete = Path.Combine(folderPath, $"{fileNamePrefix}.png");
            if (System.IO.File.Exists(pngToDelete))
            {
                System.IO.File.Delete(pngToDelete);
            }
            string jpegToDelete = Path.Combine(folderPath, $"{fileNamePrefix}.jpeg");
            if (System.IO.File.Exists(jpegToDelete))
            {
                System.IO.File.Delete(jpegToDelete);
            }

            // 儲存新的 .jpg 檔案
            string newJpgFileName = $"{fileNamePrefix}.jpg";
            var savePath = Path.Combine(folderPath, newJpgFileName);
            await System.IO.File.WriteAllBytesAsync(savePath, bytes);

            return "/images/" + newJpgFileName;
        }
    }
}
