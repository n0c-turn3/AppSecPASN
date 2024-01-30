using System.ComponentModel.DataAnnotations;

namespace AppSecPASN.ViewModels
{
	public class Register
	{
        [Required, DataType(DataType.Text), RegularExpression(@"[a-zA-Z. ]+$", ErrorMessage = "Invalid characters in first name.")]
        public string FirstName { get; set; } = string.Empty;
		[Required, DataType(DataType.Text), RegularExpression(@"[a-zA-Z. ]+$", ErrorMessage = "Invalid characters in last name.")]
		public string LastName { get; set; } = string.Empty;
        [Required, DataType(DataType.Text), RegularExpression(@"[a-zA-Z]+$", ErrorMessage = "Invalid characters in gender.")]
        public string Gender { get; set; } = string.Empty;
        [Required, DataType(DataType.Text), RegularExpression(@"[a-zA-Z0-9]+$", ErrorMessage = "Invalid characters in NRIC.")]
        public string NRIC { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please select a date for your birthday."), DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }
        [Required, DataType(DataType.Upload)]
        public IFormFile? ResumeURL { get; set; }
        [Required, DataType(DataType.Text), RegularExpression(@"[a-zA-Z0-9!@#$%^&*?_~£()., ]+$", ErrorMessage = "Invalid characters in About Me.")]
        public string AboutMe { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), RegularExpression(@"[-a-zA-Z0-9!@#$%^&*?_~£()., ]+$", ErrorMessage = "Invalid characters in password.")]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match"), RegularExpression(@"[-a-zA-Z0-9!@#$%^&*?_~£().,]+$", ErrorMessage = "Invalid characters in confirm password.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required, DataType(DataType.EmailAddress), RegularExpression(@"[-a-zA-Z0-9@._ ]+$", ErrorMessage = "Invalid characters in email.")]
        public string Email { get; set; } = string.Empty;
    }
}
