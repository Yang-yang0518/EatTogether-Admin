using System.ComponentModel.DataAnnotations;

namespace EatTogether.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "{0} 是必填欄位")]
		[Display(Name = "帳號")]
		public string Account { get; set; } = "";

		[Required(ErrorMessage = "{0} 是必填欄位")]
		[DataType(DataType.Password)]
		[Display(Name = "密碼")]
		public string Password { get; set; } = "";
	}
}
