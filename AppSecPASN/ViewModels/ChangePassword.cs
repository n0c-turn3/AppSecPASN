using System.ComponentModel.DataAnnotations;

namespace AppSecPASN.ViewModels
{
	public class ChangePassword
	{
		[Required, DataType(DataType.Password)]
		public string OldPassword { get; set; } = string.Empty;
		[Required, DataType(DataType.Password), RegularExpression(@"[-a-zA-Z0-9!@#$%^&*?_~£().,]+$", ErrorMessage = "Invalid characters in password.")]
		public string NewPassword { get; set; } = string.Empty;
		[Required, DataType(DataType.Password), Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
