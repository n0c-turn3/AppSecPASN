using AppSecPASN.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppSecPASN.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IHttpContextAccessor httpContextAccessor;

		public LogoutModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
		{
			this.signInManager = signInManager;
			this.httpContextAccessor = httpContextAccessor;
		}
		public async Task<IActionResult> OnPostLogoutAsync()
		{
			await signInManager.SignOutAsync();

			httpContextAccessor.HttpContext.Session.Remove("username");
			httpContextAccessor.HttpContext.Session.Remove("nric");
			httpContextAccessor.HttpContext.Session.Remove("email");

			Response.Cookies.Delete(".AspNetCore.Session");
			Response.Cookies.Delete(".AspNetCore.Identity.Application");
			
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
		public void OnGet()
        {
        }
    }
}
