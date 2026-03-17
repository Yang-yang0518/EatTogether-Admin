using System.ComponentModel.DataAnnotations;

namespace EatTogether.Models.ViewModels
{
	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "{0}為必填")]
		[EmailAddress(ErrorMessage = "請輸入有效的 {0} 格式")]
		[Display(Name = "Email")]
		public string Email { get; set; } = "";
	}
}
