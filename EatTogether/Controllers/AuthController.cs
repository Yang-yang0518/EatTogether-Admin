using EatTogether.Models.Infra;
using EatTogether.Models.Services;
using EatTogether.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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
		[HttpPost]
		public async Task<IActionResult> LoginAsync(LoginViewModel vm)
		{
			throw new NotImplementedException();
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
