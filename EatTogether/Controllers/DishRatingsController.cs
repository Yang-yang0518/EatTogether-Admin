using EatTogether.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
    [AllowAnonymous]
    [Route("api/Dishes")]
    [ApiController]
    public class DishRatingsController : ControllerBase
    {
        private readonly DishService _dishService;

        public DishRatingsController(DishService dishService)
        {
            _dishService = dishService;
        }

        [HttpPost("{id}/Rate")]
        public async Task<IActionResult> Rate(int id, [FromBody] RateRequest request)
        {
            if (request.Score < 1 || request.Score > 5)
                return BadRequest("score 必須介於 1 到 5 之間");

            var result = await _dishService.RateAsync(id, request.Score);
            if (result == null)
                return NotFound();

            return Ok(new
            {
                averageScore = result.Value.averageScore,
                ratingCount  = result.Value.ratingCount
            });
        }
    }

    public class RateRequest
    {
        public int Score { get; set; }
    }
}
