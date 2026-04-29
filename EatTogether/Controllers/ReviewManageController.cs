using EatTogether.Models.EfModels;
using EatTogether.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Controllers
{
    [Authorize]
    [Route("Reviews")]
    public class ReviewManageController : Controller
    {
        private readonly EatTogetherDBContext _context;

        public ReviewManageController(EatTogetherDBContext context)
        {
            _context = context;
        }

        // GET /Reviews
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string search)
        {
            var query = _context.Reviews
                .Include(r => r.Dish)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(r => r.Dish.DishName.Contains(search));

            var reviews = await query
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewManageViewModel
                {
                    Id        = r.Id,
                    DishName  = r.Dish.DishName,
                    Nickname  = r.Nickname,
                    Content   = r.Content,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            ViewBag.Search  = search;
            ViewBag.Success = TempData["Success"];

            return View(reviews);
        }

        // POST /Reviews/Delete/{id}
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                TempData["Success"] = "留言已成功刪除。";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
