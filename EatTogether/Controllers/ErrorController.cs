using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
	public class ErrorController : Controller
	{
		// GET /Error/403
		[Route("Error/403")]
		public IActionResult Forbidden()
		{
			// 從 JWT Payload 取得角色名稱
			var rolenames = User.Claims
				.FirstOrDefault(c => c.Type == "roleNames")?.Value ?? "未知角色";

			ViewBag.RoleName = rolenames;

			return View();
		}

		// GET /Error/404
		[Route("Error/404")]
		public IActionResult NotFound()
		{
			return View();
		}
	}
}
