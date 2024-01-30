using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSecPASN.Models
{
    public class Password
    {
        public int Id { get; set; }
        public string PasswordValue { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
		[Column(TypeName = "datetime2")]
		public DateTime CreatedAt { get; set; }
		[Column(TypeName = "datetime2")]
		public DateTime UpdatedAt { get; set; }
	}
}
