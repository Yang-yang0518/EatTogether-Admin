using EatTogether.Models.DTOs;
using EatTogether.Models.Infra;
using EatTogether.Models.Repositories;

namespace EatTogether.Models.Services
{
	public interface IAuthService
	{
		Task<Result<LoginDto>> ForceChangePasswordAsync(int userId, string newPassword);
		Task<Result<LoginDto>> LoginAsync(string account, string password);
	}

	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepo;
		private readonly IRoleRepository _roleRepo;

		public AuthService(IUserRepository userRepo, IRoleRepository roleRepo)
		{
			_userRepo = userRepo;
			_roleRepo = roleRepo;
		}

		public async Task<Result<LoginDto>> LoginAsync(string account, string password)
		{
			// 1. 用帳號查使用者
			var user = await _userRepo.GetByAccountAsync(account);

			// 2. 帳號不存在 or 密碼錯誤 → 一律回傳同一訊息（防帳號枚舉）
			if (user == null || !HashUtility.VerifyPassword(password, user.HashedPassword))
				return Result<LoginDto>.Fail("帳號或密碼錯誤");

			// 3. 已刪除的帳號 → 同樣回傳「帳號或密碼錯誤」（不透露帳號存在）
			if (user.IsDeleted)
				return Result<LoginDto>.Fail("帳號或密碼錯誤");

			// 4. 帳號停用 → 顯示明確訊息
			if (!user.IsActive)
				return Result<LoginDto>.Fail("此帳號已停用，請聯絡店長");

			var roleNames = await _roleRepo.GetRoleNamesByIdsAsync(user.RoleIds);

			// 5. 驗證通過 → 組裝 LoginDto 回傳
			var loginDto = new LoginDto
			{
				UserId = user.Id,
				Account = user.Account,
				Name = user.Name,
				RoleIds = user.RoleIds,
				RoleNames = roleNames,
				MustChangePassword = user.MustChangePassword
			};

			return Result<LoginDto>.Success(loginDto);
		}

		public async Task<Result<LoginDto>> ForceChangePasswordAsync(int userId, string newPassword)
		{
			// 先確認使用者存在
			var user = await _userRepo.GetByIdAsync(userId);
			if (user == null) return Result<LoginDto>.Fail("使用者不存在");

			// 更新 HashedPassword
			var hashedPassword = HashUtility.HashPassword(newPassword);
			await _userRepo.UpdatePasswordAsync(userId, hashedPassword);

			// MustChangePassword 設為 0
			await _userRepo.SetMustChangePasswordAsync(userId, false);

			// 組裝 LoginDto
			var roleNames = await _roleRepo.GetRoleNamesByIdsAsync(user.RoleIds);

			var loginDto = new LoginDto
			{
				UserId = user.Id,
				Account = user.Account,
				Name = user.Name,
				RoleIds = user.RoleIds,
				RoleNames = roleNames,
				MustChangePassword = false
			};

			return Result<LoginDto>.Success(loginDto);

		}
	}
}
