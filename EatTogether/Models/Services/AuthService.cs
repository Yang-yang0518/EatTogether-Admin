using EatTogether.Models.DTOs;
using EatTogether.Models.Infra;
using EatTogether.Models.Repositories;
using MimeKit.Cryptography;

namespace EatTogether.Models.Services
{
	public interface IAuthService
	{
		Task<Result<LoginDto>> LoginAsync(string account, string password);
	}

	public class AuthService : IAuthService
	{
		private readonly IUserRepository _repo;

		public AuthService(IUserRepository repo)
		{
			_repo = repo;
		}

		public async Task<Result<LoginDto>> LoginAsync(string account, string password)
		{
			// 1. 用帳號查使用者
			var user = await _repo.GetByAccountAsync(account);

			// 2. 帳號不存在 or 密碼錯誤 → 一律回傳同一訊息（防帳號枚舉）
			if (user == null || !HashUtility.VerifyPassword(password, user.HashedPassword))
				return Result<LoginDto>.Fail("帳號或密碼錯誤");

			// 3. 已刪除的帳號 → 同樣回傳「帳號或密碼錯誤」（不透露帳號存在）
			if (user.IsDeleted)
				return Result<LoginDto>.Fail("帳號或密碼錯誤");

			// 4. 帳號停用 → 顯示明確訊息
			if (!user.IsActive)
				return Result<LoginDto>.Fail("此帳號已停用，請聯絡店長");

			// 5. 驗證通過 → 組裝 LoginDto 回傳
			var loginDto = new LoginDto
			{
				UserId = user.Id,
				Account = user.Account,
				Name = user.Name,
				RoleIds = user.RoleIds,
				MustChangePassword = user.MustChangePassword
			};

			return Result<LoginDto>.Success(loginDto);
		}
	}
}
