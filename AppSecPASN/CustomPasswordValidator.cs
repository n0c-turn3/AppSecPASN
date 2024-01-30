using AppSecPASN.Models;
using Microsoft.AspNetCore.Identity;

public class CustomPasswordValidator : IPasswordValidator<ApplicationUser>
{
	public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
	{
		return Task.Run(() =>
		{
			if (!password.Any(c => "-!@#$%^&*?_~£().,".Contains(c)))
			{
				return IdentityResult.Failed(new IdentityError
				{
					Code = "NONALPHANUMERIC",
					Description = "Password should contain at least one of the following non-alphanumeric characters: -!@#$%^&*?_~£().,"
				});
			}
			else
			{
				return IdentityResult.Success;
			}
		});
	}
}