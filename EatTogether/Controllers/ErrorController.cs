using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
    // 如果這是 API 專案，建議加上 [ApiController] 確保行為一致
    [Route("api/[controller]")]
    public class ErrorController : Controller // 或是繼承 ControllerBase
    {
        // 1. 修正 Forbidden：移除衝突的 Route 或標註
        [HttpGet("403")]
        public IActionResult Forbidden()
        {
            var rolenames = User.Claims
                .FirstOrDefault(c => c.Type == "roleNames")?.Value ?? "未知角色";

            ViewBag.RoleName = rolenames;
            return View();
        }

        // 2. 修正 NotFound：💡 必須補上 [HttpGet]
        [HttpGet("404")]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
