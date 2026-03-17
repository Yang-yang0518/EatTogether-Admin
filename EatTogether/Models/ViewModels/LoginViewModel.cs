using System.ComponentModel.DataAnnotations;

namespace EatTogether.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "{0}為必填")]
		[Display(Name = "帳號")]
		public string Account { get; set; } = "";

		[Required(ErrorMessage = "{0}為必填")]
		[DataType(DataType.Password)]
		[Display(Name = "密碼")]
		public string Password { get; set; } = "";
	}
}
