namespace EatTogether.Models.Infra
{
	public static class HashUtility
	{
		private const int workFactor = 12;

		/// <summary>
		/// 將明文密碼進行 BCrypt 雜湊處理 (會自動生成並混入 Salt)
		/// </summary>
		/// <param name="password">使用者輸入的明文密碼</param>
		/// <returns>包含 Salt 與雜湊結果的字串</returns>
		public static string HashPassword(string password)
		{
			if(string.IsNullOrEmpty(password))
			{
				throw new ArgumentException("密碼不能為空", nameof(password));
			}

			return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
		}

		/// <summary>
		/// 驗證明文密碼與資料庫中的雜湊密碼是否相符
		/// </summary>
		/// <param name="password">使用者登入時輸入的明文密碼</param>
		/// <param name="hashedPassword">從資料庫取出的雜湊密碼</param>
		/// <returns>密碼是否正確</returns>
		public static bool VerifyPassword(string password, string hashedPassword)
		{
			if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
			{
				return false;
			}

			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}
	}
}
