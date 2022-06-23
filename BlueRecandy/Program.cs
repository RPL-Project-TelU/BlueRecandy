using BlueRecandy.Data;
using BlueRecandy.Models;
using BlueRecandy.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");;


// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Service
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IFeedbacksService, FeedbacksService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPurchaseLogsService, PurchaseLogsService>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddViewLocalization();

builder.Services.AddPortableObjectLocalization()
	.Configure<RequestLocalizationOptions>(options =>
	{
		var supportedCultures = new List<CultureInfo>
		{
			new CultureInfo("en"),
			new CultureInfo("id"),
			new CultureInfo("fr"),
			new CultureInfo("es"),
			new CultureInfo("ja")
		};

		options.DefaultRequestCulture = new RequestCulture("en");
		options.SupportedCultures = supportedCultures;
		options.SupportedUICultures = supportedCultures;

	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
