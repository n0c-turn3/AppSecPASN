using System.ComponentModel.DataAnnotations;

namespace AppSecPASN.ViewModels
{
	public class Login
	{
		[Required, DataType(DataType.EmailAddress), RegularExpression(@"[-a-zA-Z0-9@._]+$", ErrorMessage = "Invalid characters in email.")]
		public string Email { get; set; }
		[Required, DataType(DataType.Password), RegularExpression(@"[-a-zA-Z0-9!@#$%^&*?_~£().,]+$", ErrorMessage = "Invalid characters in password.")]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
