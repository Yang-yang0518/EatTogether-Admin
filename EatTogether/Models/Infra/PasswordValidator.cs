namespace EatTogether.Models.Infra
{
	public static class PasswordValidator
	{
		/// <summary>
		/// 驗證密碼是否符合規則：至少 6 碼，且同時包含英文字母與數字
		/// </summary>
		/// <param name="password">要驗證的密碼</param>
		/// <returns>密碼是否符合規則</returns>
		public static bool IsValid(string password)
		{
			if (string.IsNullOrEmpty(password)) return false;
			if (password.Length < 6) return false;

			bool hasLetter = password.Any(char.IsLetter);
			bool hasDigit = password.Any(char.IsDigit);

			return hasLetter && hasDigit;
		}
	}
}
