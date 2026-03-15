using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EatTogether.Models.Infra
{
	public class JwtHelper
	{
		private readonly IConfiguration _config;

		public JwtHelper(IConfiguration config)
		{
			_config = config;
		}

		/// <summary>
		/// 產生 JWT，Payload 包含 UserId 與 RoleIds 清單
		/// </summary>
		public string GenerateToken(int userId, List<int> roleIds)
		{
			var jwtSettings = _config.GetSection("Jwt");
			var secretKey = jwtSettings["SecretKey"]!;
			var issuer = jwtSettings["Issuer"]!;
			var audience = jwtSettings["Audience"]!;

			var claims = new List<Claim>
			{
				new Claim("userId", userId.ToString()),
				// 多個角色各自寫成一筆 roleId Claim
				// → RequirePermissionAttribute 可用 claims.Where("roleId") 取得清單
			};

			foreach (var roleId in roleIds)
			{
				claims.Add(new Claim("roleId", roleId.ToString()));
			}

			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddHours(8),   // 效期 8 小時
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
