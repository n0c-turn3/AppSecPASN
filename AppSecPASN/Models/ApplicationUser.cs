using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSecPASN.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public string NRIC { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
		public string ResumeURL { get; set; } = string.Empty;
		public string AboutMe { get; set; } = string.Empty;
        public List<Password>? Passwords { get; set; }
    }
}
