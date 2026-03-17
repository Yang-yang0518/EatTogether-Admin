using EatTogether.Models.DTOs;
using EatTogether.Models.EfModels;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{
	public interface IUserRepository
	{
		Task<UserDto?> GetByAccountAsync(string account);
		Task<UserDto?> GetByIdAsync(int userId);
		Task SetMustChangePasswordAsync(int userId, bool value);
		Task UpdatePasswordAsync(int userId, string hashedPassword);
	}

	public class UserRepository : IUserRepository
	{
		private readonly EatTogetherDBContext _context;

		public UserRepository(EatTogetherDBContext context)
		{
			_context = context;
		}

		public async Task<UserDto?> GetByAccountAsync(string account)
		{
			var user = await _context.Users
				.AsNoTracking()
				.Where(u => u.Account == account)
				.Select(u => new UserDto
				{
					Id = u.Id,
					Account = u.Account,
					HashedPassword = u.HashedPassword,
					Name = u.Name,
					IsActive = u.IsActive,
					IsDeleted = u.IsDeleted,
					MustChangePassword = u.MustChangePassword,
					RoleIds = u.UserRoles.Select(ur => ur.RoleId).ToList()
				})
				.FirstOrDefaultAsync();

			return user;
		}

		public async Task<UserDto?> GetByIdAsync(int userId)
		{
			var user = await _context.Users
				.AsNoTracking()
				.Where(u => u.Id == userId)
				.Select(u => new UserDto
				{
					Id = u.Id,
					Account = u.Account,
					HashedPassword = u.HashedPassword,
					Name = u.Name,
					IsActive = u.IsActive,
					IsDeleted = u.IsDeleted,
					MustChangePassword = u.MustChangePassword,
					RoleIds = u.UserRoles.Select(ur => ur.RoleId).ToList()
				})
				.FirstOrDefaultAsync();

			return user;
		}

		public async Task UpdatePasswordAsync(int userId, string hashedPassword)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user == null) return;

			user.HashedPassword = hashedPassword;
			await _context.SaveChangesAsync();
		}

		public async Task SetMustChangePasswordAsync(int userId, bool value)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user == null) return;

			user.MustChangePassword = value;
			await _context.SaveChangesAsync();
		}
	}
}
