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
        private readonly CategoryService _categoryService;

        public SetMealsController(SetMealService setMealService, DishService dishService, CategoryService categoryService)
        {
            _setMealService = setMealService;
            _dishService    = dishService;
            _categoryService = categoryService;
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
            
            // Prepare data for the new UI
            var vm = new SetMealViewModel { DisplayOrder = nextOrder };
            await PopulateCategoriesWithDishes(vm);
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SetMealViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesWithDishes(vm); // Repopulate if validation fails
                return View(vm);
            }

            if (!string.IsNullOrEmpty(vm.CroppedImageData))
                vm.ImageUrl = await SaveBase64ImageAsync(vm.CroppedImageData, vm.SetMealName);

            await _setMealService.CreateAsync(vm.ToDto());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _setMealService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = dto.ToViewModel();

            // Populate CategoriesWithDishes for the new UI
            await PopulateCategoriesWithDishes(vm);
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] SetMealViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesWithDishes(vm); // Repopulate if validation fails
                return View(vm);
            }

            if (!string.IsNullOrEmpty(vm.CroppedImageData))
            {
                // 強制覆蓋原有檔案，並用餐點名稱命名
                vm.ImageUrl = await SaveBase64ImageAsync(vm.CroppedImageData, vm.SetMealName);
            }

            await _setMealService.UpdateAsync(vm.ToDto());
            return RedirectToAction(nameof(Index));
        }
        
        // Helper method to populate CategoriesWithDishes
        private async Task PopulateCategoriesWithDishes(SetMealViewModel vm)
        {
            var allCategories = await _categoryService.GetAllAsync();
            var allActiveDishes = await _dishService.GetAllAsync(); // This already filters for IsActive

            var categoriesWithDishes = new List<CategoryWithDishesViewModel>();

            foreach (var category in allCategories.OrderBy(c => c.DisplayOrder)) // Assuming categories have DisplayOrder
            {
                var categoryVm = new CategoryWithDishesViewModel
                {
                    CategoryId = category.Id,
                    CategoryName = category.CategoryName,
                    DishesInThisCategory = allActiveDishes
                        .Where(d => d.CategoryId == category.Id)
                        .Select(d => new SelectListItem
                        {
                            Value = d.Id.ToString(),
                            Text = $"{d.DishName} (${d.Price})",
                            Selected = vm.Items.Any(item => item.DishId == d.Id) // Pre-select in dropdown if already in set meal
                        }).ToList()
                };

                // Populate SelectedItemsForCategory for rendering existing items
                categoryVm.SelectedItemsForCategory = vm.Items
                    .Where(item => allActiveDishes.Any(d => d.Id == item.DishId && d.CategoryId == category.Id))
                    .ToList();
                
                // Set category-level optionality based on the first optional item in this category (if any)
                var firstOptionalItem = categoryVm.SelectedItemsForCategory.FirstOrDefault(i => i.IsOptional);
                if (firstOptionalItem != null)
                {
                    categoryVm.IsCategoryOptional = true;
                    categoryVm.OptionGroupNoForCategory = firstOptionalItem.OptionGroupNo;
                    categoryVm.PickLimitForCategory = firstOptionalItem.PickLimit;
                }

                categoriesWithDishes.Add(categoryVm);
            }
            vm.CategoriesWithDishes = categoriesWithDishes;
        }

        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            await _setMealService.DisableAsync(id);
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

        [HttpPost("SetMeals/UpdateItems/{setMealId}")]
        public async Task<IActionResult> UpdateItems(int setMealId, [FromBody] List<SetMealItemViewModel> items)
        {
            if (items == null) return BadRequest("無項目可更新。");

            try
            {
                var dtos = items.Select(i => i.ToItemDto());
                await _setMealService.UpdateItemsAsync(setMealId, dtos);
                return Ok(new { message = "套餐內容更新成功！" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { message = "更新失敗：" + ex.Message });
            }
        }
    }
}
