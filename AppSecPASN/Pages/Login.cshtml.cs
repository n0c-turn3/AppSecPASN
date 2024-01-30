using AppSecPASN.Models;
using AppSecPASN.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Web;

namespace AppSecPASN.Pages
{
    public class LoginModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
		private readonly ILogger<LoginModel> logger;

		public LoginModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger)
        {
			this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
			this.logger = logger;
		}
        [BindProperty]
        public Login LModel { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Set on lock out later
                var loginResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, lockoutOnFailure: true);
				if (loginResult.Succeeded)
                {
                    var dataProtectorProvider = DataProtectionProvider.Create("EncryptData");
                    var protector = dataProtectorProvider.CreateProtector("MySecretKey");
                    List<Claim> claims = new()
                    {
                        new Claim(ClaimTypes.Email, LModel.Email)
                    };
                    ClaimsIdentity identity = new(claims, "MyCookieAuth");
                    ClaimsPrincipal claimsPrincipal = new(identity);
                    await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                    var user = await userManager.FindByEmailAsync(LModel.Email);
                    var nric = protector.Unprotect(user.NRIC);

                    httpContextAccessor.HttpContext.Session.SetString("username", user.FirstName + " " + user.LastName);
                    httpContextAccessor.HttpContext.Session.SetString("nric", nric);
                    httpContextAccessor.HttpContext.Session.SetString("email", LModel.Email);

                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1),
                        Secure = true,
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                    };
                    // Response.Cookies.Append("username", user.FirstName + " " + user.LastName, cookieOptions);
                    // Response.Cookies.Append("nric", nric, cookieOptions);

					return RedirectToPage("Index");
                }
                if (loginResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "You have been locked out. Please try again later.");
                } else
                {
                    ModelState.AddModelError("", "Username or Password invalid");
                }
                
            }
            return Page();
        }
        public void OnGet()
        {
        }
    }
}
