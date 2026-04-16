using EatTogether.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
    [AllowAnonymous]
    [Route("api/Dishes")]
    [ApiController]
    public class DishStockController : ControllerBase
    {
        private readonly DishService _dishService;

        public DishStockController(DishService dishService)
        {
            _dishService = dishService;
        }

        // PATCH /api/Dishes/{id}/Stock
        [HttpPatch("{id}/Stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequest request)
        {
            if (request.StockStatus != 0 && request.StockStatus != 1 && request.StockStatus != 2)
                return BadRequest("stockStatus 必須是 0（供應中）、1（剩餘不多）或 2（售完）");

            var found = await _dishService.UpdateStockAsync(id, request.StockStatus);
            if (!found)
                return NotFound();

            return Ok(new
            {
                id          = id,
                stockStatus = request.StockStatus
            });
        }
    }

    public class UpdateStockRequest
    {
        public int StockStatus { get; set; }
    }
}
