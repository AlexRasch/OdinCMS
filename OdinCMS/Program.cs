using Microsoft.EntityFrameworkCore;
using OdinCMS.DataAccess.Data;
using OdinCMS.DataAccess.Repository;
using OdinCMS.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using OdinCMS.Utility;
using Stripe;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Fake email
builder.Services.AddSingleton<IEmailSender, EmailSender>();
// 
builder.Services.AddRazorPages();

// API keys
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

// Stripe
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Stripe
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();


// This need to be in this order
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
