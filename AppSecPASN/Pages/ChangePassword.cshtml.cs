using AppSecPASN.Models;
using AppSecPASN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AppSecPASN.Pages
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
		private readonly AppAuthDbContext context;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly ILogger<ChangePasswordModel> logger;

		public ChangePasswordModel(AppAuthDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor,
            ILogger<ChangePasswordModel> logger)
        {
			this.context = context;
			this.userManager = userManager;
			this.httpContextAccessor = httpContextAccessor;
			this.logger = logger;
		}
        [BindProperty]
        public ChangePassword CModel { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var htmlEncoder = System.Text.Encodings.Web.HtmlEncoder.Default;
                CModel.NewPassword = htmlEncoder.Encode(CModel.NewPassword.Trim());
                CModel.ConfirmPassword = htmlEncoder.Encode(CModel.ConfirmPassword.Trim());

				var userEmail = httpContextAccessor.HttpContext.Session.GetString("email");
				var user = await userManager.FindByEmailAsync(userEmail);
				List<Password> passwords = context.Passwords
                    .Where(p => p.UserId == user.Id).OrderBy(p => p.CreatedAt).ToList();

                TimeSpan changeDifference;
                if (passwords.Count > 1)
                {
                    changeDifference = DateTime.Now - passwords[1].CreatedAt;
                } else
                {
                    changeDifference = DateTime.Now - passwords[0].CreatedAt;
                }
                if (changeDifference.TotalMinutes < 1 && passwords.Count > 1)
                {
                    ModelState.AddModelError("", "You just changed your password. Please wait awhile.");
                    return Page();
                }

                foreach (var password in passwords)
                {
                    if (password.PasswordValue.Equals(CModel.NewPassword))
                    {
                        ModelState.AddModelError("", "You cannot use your last two passwords.");
                        return Page();
                    }
                }

                var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, CModel.OldPassword);
                if (isCurrentPasswordValid)
                {
                    var result = await userManager.ChangePasswordAsync(user, CModel.OldPassword, CModel.NewPassword);
                    if (result.Succeeded)
                    {
                        // If successfully change password, remove the first item from the list, i.e. the earliest password entry
                        if (passwords.Count > 1)
                        {
							context.Passwords.Remove(passwords[0]);
						}
                        context.Passwords.Add(new Password
                        {
                            PasswordValue = CModel.NewPassword, 
                            UserId = user.Id,
							CreatedAt = DateTime.Now,
							UpdatedAt = DateTime.Now
						});
                        await context.SaveChangesAsync();
                        return RedirectToPage("Index");
                    } else
                    {
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
                } else
                {
                    ModelState.AddModelError("", "Your old password is incorrect. Please try again");
                    return Page();
                }
            }
			return Page();
        }
        public void OnGet()
        {
        }
    }
}
