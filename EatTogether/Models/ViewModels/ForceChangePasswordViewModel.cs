using System.ComponentModel.DataAnnotations;

namespace EatTogether.Models.ViewModels
{
	public class ForceChangePasswordViewModel
	{
		[Required(ErrorMessage = "{0}為必填")]
		[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
			ErrorMessage = "密碼須至少 6 碼，並包含英文與數字")]
		[DataType(DataType.Password)]
		[Display(Name = "新密碼")]
		public string NewPassword { get; set; } = "";

		[Required(ErrorMessage = "{0}為必填")]
		[Compare("NewPassword", ErrorMessage = "兩次密碼不一致")]
		[DataType(DataType.Password)]
		[Display(Name = "確認密碼")]
		public string ConfirmPassword { get; set; } = "";
	}
}
