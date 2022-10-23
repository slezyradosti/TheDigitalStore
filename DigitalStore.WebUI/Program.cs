using DigitalStore.EF;
using DigitalStore.Repos;
using DigitalStore.Models.NotForDB;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using Microsoft.Extensions.DependencyInjection;
using DigitalStore.Repos.Interfaces;
using DigitalStore.BusinessLogic.Interfaces;
using DigitalStore.BusinessLogic;
using Microsoft.AspNetCore.Identity;
using DigitalStore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DigitalStore.WebUI.Areas.Identity.Pages.Account.Manage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DigitalStoreContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DigitalStoreContext>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddPaging();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IOrderProcessor, EmailOrderProcessor>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IProductOrderRepo, ProductOrderRepo>();
builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddScoped<EmailSettings, EmailSettings>();
builder.Services.AddTransient<IOrderProcessor, EmailOrderProcessor>();
builder.Services.AddTransient<IProductOrderLogic, ProductOrderLogic>();
builder.Services.AddTransient<IOrderLogic, OrderLogic>();

//for authorization on claim-base principial
builder.Services.AddAuthorization(options =>
 {
    options.AddPolicy("AdminAccess",
    policyBuilder => policyBuilder
    .RequireClaim("CanUseAdminPanel"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
