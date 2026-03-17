using EatTogether.Models.DTOs;
using EatTogether.Models.Infra;
using EatTogether.Models.Services;
using EatTogether.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly JwtHelper _jwtHelper;

		public AuthController(IAuthService authService, JwtHelper jwtHelper)
		{
			_authService = authService;
			_jwtHelper = jwtHelper;
		}


		// POST /Auth/Login
		[HttpGet]
		public async Task<IActionResult> Login(LoginViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.FirstOrDefault();
				return Json(new { success = false, message = errors });
			}

			var result = await _authService.LoginAsync(vm.Account, vm.Password);

			if (!result.IsSuccess)
			{
				return Json(new { success= false, message = result.ErrorMessage });
			}

			var loginDto = result.Value!;

			if (loginDto.MustChangePassword)
			{
				TempData["PendingUserId"] = loginDto.UserId;
				return Json(new { success = true, mustChangePassword = true });
			}

			IssueJwtCookie(loginDto);

			return Json(new { success = true, mustChangePassword = false });
		}

		private void IssueJwtCookie(LoginDto loginDto)
		{
			var playloadDto = new JwtPayloadDto
			{
				UserId = loginDto.UserId,
				RoleIds = loginDto.RoleIds,
				Name = loginDto.Name,
				RoleNames = loginDto.RoleNames
			};

			var token = _jwtHelper.GenerateToken(playloadDto);
			Response.Cookies.Append("jwt", token, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTimeOffset.UtcNow.AddHours(8)
			});
		}

		// GET /Auth/ResetPassword
		[HttpGet]
		public IActionResult ResetPassword(string token)
		{
			if (string.IsNullOrEmpty(token))
				return RedirectToAction("ResetPasswordInvalid");

			ViewBag.Token = token;
			return View();
		}

		// GET /Auth/ResetPasswordInvalid
		[HttpGet]
		public IActionResult ResetPasswordInvalid() => View();
	}
}
