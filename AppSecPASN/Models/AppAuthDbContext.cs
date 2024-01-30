using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppSecPASN.Models
{
	public class AppAuthDbContext : IdentityDbContext<ApplicationUser>
	{
		private readonly IConfiguration _configuration;
		//public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){ }
		public AppAuthDbContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string connectionString = _configuration.GetConnectionString("AuthConnectionString");
			if (connectionString != null)
			{
				optionsBuilder.UseSqlServer(connectionString);
			}
		}
        public DbSet<Password> Passwords { get; set; }
    }
}
