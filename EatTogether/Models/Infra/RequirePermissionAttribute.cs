using EatTogether.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EatTogether.Models.Infra
{
	public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
	{
		private readonly string _functionName;

		public RequirePermissionAttribute(string functionName)
		{
			_functionName = functionName;
		}

		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			var user = context.HttpContext.User;

			// 未登入 → 導向登入頁
			if(user?.Identity?.IsAuthenticated != true)
			{
				context.Result = new RedirectToActionResult("Login", "Auth", null);
				return;
			}

			// 從 JWT Payload 取 roleId 清單
			var roleIds = user.Claims
				.Where(c => c.Type == "roleId")
				.Select(c => int.Parse(c.Value))
				.ToList();

			// 查詢 RoleFunctions，取得該角色聯集的所有 FunctionName
			var roleFuncRepo = context.HttpContext.RequestServices.GetRequiredService<IRoleFunctionRepository>();

			var allowedFunctions = await roleFuncRepo.GetFunctionNamesByRoleIdsAsync(roleIds);

			if (!allowedFunctions.Contains(_functionName))
			{
				context.Result = new StatusCodeResult(403);
			}
		}
	}
}
