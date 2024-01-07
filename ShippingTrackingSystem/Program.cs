using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.BackEnd.Interfaces;
using ShippingTrackingSystem.BackEnd.Repository;
using ShippingTrackingSystem.Models;
using ShippingTrackingSystem.Models.Context;
using ShippingTrackingSystem.Services.Interfaces;
using ShippingTrackingSystem.Services.Repository;

var builder = WebApplication.CreateBuilder(args);

// to get the current user.
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Fore Views
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Add services to the container. client side validation
builder.Services.AddControllersWithViews(
    options =>
    {
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "This field is required");
    });

//For connection to database
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

// Identity Managers
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // to avoid the email confirmation ...
})
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();


// Services
builder.Services.AddScoped<IFileService, FileService>();

// Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ISystemStatisticsRepository, SystemStatisticsRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.Run();
