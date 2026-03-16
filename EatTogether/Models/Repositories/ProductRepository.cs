using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EatTogetherDBContext _context;

        public ProductRepository(EatTogetherDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Dish)
                    .ThenInclude(d => d != null ? d.Category : null)
                .Include(p => p.SetMeal)
                .Select(p => new ProductDto
                {
                    Id                = p.Id,
                    ProductType       = p.ProductType,
                    DishId            = p.DishId,
                    DishName          = p.Dish != null ? p.Dish.DishName : null,
                    DishPrice         = p.Dish != null ? p.Dish.Price : null,
                    DishImageUrl      = p.Dish != null ? p.Dish.ImageUrl : null,
                    DishCategoryName  = p.Dish != null && p.Dish.Category != null
                                        ? p.Dish.Category.CategoryName : null,
                    SetMealId         = p.SetMealId,
                    SetMealName       = p.SetMeal != null ? p.SetMeal.SetMealName : null,
                    SetMealPrice      = p.SetMeal != null ? p.SetMeal.SetPrice : null,
                    SetMealImageUrl   = p.SetMeal != null ? p.SetMeal.ImageUrl : null
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Dish)
                    .ThenInclude(d => d != null ? d.Category : null)
                .Include(p => p.SetMeal)
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id                = p.Id,
                    ProductType       = p.ProductType,
                    DishId            = p.DishId,
                    DishName          = p.Dish != null ? p.Dish.DishName : null,
                    DishPrice         = p.Dish != null ? p.Dish.Price : null,
                    DishImageUrl      = p.Dish != null ? p.Dish.ImageUrl : null,
                    DishCategoryName  = p.Dish != null && p.Dish.Category != null
                                        ? p.Dish.Category.CategoryName : null,
                    SetMealId         = p.SetMealId,
                    SetMealName       = p.SetMeal != null ? p.SetMeal.SetMealName : null,
                    SetMealPrice      = p.SetMeal != null ? p.SetMeal.SetPrice : null,
                    SetMealImageUrl   = p.SetMeal != null ? p.SetMeal.ImageUrl : null
                })
                .FirstOrDefaultAsync();
        }
    }
}
