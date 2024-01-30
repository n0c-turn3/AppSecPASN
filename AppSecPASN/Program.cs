using AppSecPASN;
using AppSecPASN.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppAuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.Password.RequiredLength = 12;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	// Set a custom check for this requirement
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireDigit = true;

	options.Lockout.AllowedForNewUsers = true;
	// Lockout time set to 30 seconds to facilitate testing
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
	options.Lockout.MaxFailedAccessAttempts = 3;
}).AddEntityFrameworkStores<AppAuthDbContext>().AddPasswordValidator<CustomPasswordValidator>();
builder.Services.AddDataProtection();

builder.Services.ConfigureApplicationCookie(config =>
{
	config.LoginPath = "/Login";
});
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
	options.Cookie.Name = "MyCookieAuth";
	options.AccessDeniedPath = "/Account/AccessDenied";
	options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
});

 builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
 builder.Services.AddDistributedMemoryCache();
 builder.Services.AddSession(options =>
 {
	options.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePages(async context =>
{
	var request = context.HttpContext.Request;
	var response = context.HttpContext.Response;

	var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
	var viewPath = Path.Combine(env.ContentRootPath, $"Pages/Errors/{response.StatusCode}.cshtml");

	var viewExists = System.IO.File.Exists(viewPath);

	if (viewExists)
	{
		response.Redirect($"/Errors/{response.StatusCode}");
	} else
	{
		response.Redirect("/Errors/Default");
	}
});

// For development environments
app.UseExceptionHandler("/Errors/Default");

app.UseRouting();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401)
    {
        context.Request.Path = "/Login";
        await next();
    }
});

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
