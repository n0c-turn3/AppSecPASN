using AppSecPASN.Models;
using AppSecPASN.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppSecPASN.Pages
{
    public class RegisterModel : PageModel
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IWebHostEnvironment hostEnvironment;
        private readonly AppAuthDbContext context;

        public RegisterModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostEnvironment, AppAuthDbContext context)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.hostEnvironment = hostEnvironment;
            this.context = context;
        }
        [BindProperty]
        public Register RModel { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Escape string characters to protect against XSS attacks
                var htmlEncoder = System.Text.Encodings.Web.HtmlEncoder.Default;
                RModel.FirstName = htmlEncoder.Encode(RModel.FirstName.Trim());
                RModel.LastName = htmlEncoder.Encode(RModel.LastName.Trim());
                RModel.AboutMe = htmlEncoder.Encode(RModel.AboutMe.Trim());
                RModel.Password = htmlEncoder.Encode(RModel.Password.Trim());
                RModel.Gender = htmlEncoder.Encode(RModel.Gender.Trim());
                RModel.NRIC = htmlEncoder.Encode(RModel.NRIC.Trim());
                RModel.Email = htmlEncoder.Encode(RModel.Email.Trim());

				var dataProtectorProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectorProvider.CreateProtector("MySecretKey");
                
                if ((await userManager.FindByEmailAsync(RModel.Email)) is not null)
                {
                    ModelState.AddModelError("", "Email is already taken. Please try again.");
                    return Page();
                }   

                var passwordStrength = PasswordStrengthChecker.CheckStrength(RModel.Password);
				if (passwordStrength == PasswordStrengthChecker.PasswordScore.VeryWeak)
				{
					ModelState.AddModelError("", "Very weak password");
				}
				if (passwordStrength == PasswordStrengthChecker.PasswordScore.Weak || passwordStrength == PasswordStrengthChecker.PasswordScore.Medium)
                {
					ModelState.AddModelError("", "Weak password");
				}
				if (passwordStrength == PasswordStrengthChecker.PasswordScore.Strong)
				{
					ModelState.AddModelError("", "Strong password");
				}
				if (passwordStrength == PasswordStrengthChecker.PasswordScore.VeryStrong)
				{
					ModelState.AddModelError("", "Very strong password");
				}

				var user = new ApplicationUser
                {
                    UserName = RModel.Email,
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    Gender = RModel.Gender,
                    NRIC = protector.Protect(RModel.NRIC),
                    DateOfBirth = RModel.DateOfBirth,
                    AboutMe = RModel.AboutMe,
                    Email = RModel.Email,
                };
				if ((Path.GetExtension(RModel.ResumeURL.FileName) != ".docx") &&
						(Path.GetExtension(RModel.ResumeURL.FileName) != ".pdf"))
				{
					ModelState.AddModelError("", "Invalid file format. Please select a different file");
					return Page();
				}
				var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                    // Add password to Passwords table
                    var newUser = await userManager.FindByEmailAsync(user.Email);
                    var newPassword = new Password()
                    {
                        PasswordValue = RModel.Password,
                        UserId = newUser.Id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    await context.Passwords.AddAsync(newPassword);
                    await context.SaveChangesAsync();

                    // Copy file to file direcetory in wwwroot
                    var uploadsFolderPath = Path.Combine(hostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }
                    string uniqueFileName = GetUniqueFileName(RModel.ResumeURL.FileName);
                    string filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                    await RModel.ResumeURL.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    user.ResumeURL = uniqueFileName;
                    var updateResult = await userManager.UpdateAsync(user);
                    if (updateResult.Succeeded)
                    {
						return RedirectToPage("Login");
					}
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                + "_"
				+ Guid.NewGuid().ToString().Substring(0, 4)
                + Path.GetExtension(fileName);
        }
        public void OnGet()
        {
        }
    }
}
