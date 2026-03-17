using EatTogether.Models.EfModels;
using Microsoft.EntityFrameworkCore;

namespace EatTogether.Models.Repositories
{
	public interface IPasswordResetTokenRepository
	{
		Task CreateAsync(PasswordResetToken token);
		Task<PasswordResetToken?> GetValidTokenAsync(string token);
		Task InvalidatePreviousTokensAsync(int userId);
		Task MarkUsedAsync(int tokenId);
	}

	public class PasswordResetTokenRepository : IPasswordResetTokenRepository
	{
		private readonly EatTogetherDBContext _context;

		public PasswordResetTokenRepository(EatTogetherDBContext context)
		{
			_context = context;
		}

		// 建立新 Token，ExpiresAt = 現在 + 60 分鐘
		public async Task CreateAsync(PasswordResetToken token)
		{
			token.ExpiresAt = DateTime.Now.AddMinutes(60);
			_context.PasswordResetTokens.Add(token);
			await _context.SaveChangesAsync();
		}


		// 將該使用者舊的未使用 Token 全部標記為已使用
		public async Task InvalidatePreviousTokensAsync(int userId)
		{
			var tokens = await _context.PasswordResetTokens
				.Where(t => t.UserId == userId && !t.IsUsed)
				.ToListAsync();

			foreach (var t in tokens)
			{
				t.IsUsed = true;
			}

			await _context.SaveChangesAsync();
		}

		// 查詢有效 Token（存在 + 未使用 + 未逾期）
		public async Task<PasswordResetToken?> GetValidTokenAsync(string token)
		{
			var resetToken = await _context.PasswordResetTokens
				.AsNoTracking()
				.FirstOrDefaultAsync(t =>
					t.Token == token &&
					!t.IsUsed &&
					t.ExpiresAt > DateTime.Now
				);

			return resetToken;
		}

		// 將 Token 標記為已使用（一次性）
		public async Task MarkUsedAsync(int tokenId)
		{
			var token = await _context.PasswordResetTokens.FindAsync(tokenId);
			if (token == null) return;

			token.IsUsed = true;
			await _context.SaveChangesAsync();
		}

	}
}
