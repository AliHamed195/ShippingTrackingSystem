using Microsoft.EntityFrameworkCore;
using ShippingTrackingSystem.Models.Context;
using ShippingTrackingSystem.Services.Interfaces;
using ShippingTrackingSystem.Services.Repository;

var builder = WebApplication.CreateBuilder(args);

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

//for connection to database
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<MyDbContext>(item => item.UseSqlServer(configuration.GetConnectionString("conn")));

// Services
builder.Services.AddScoped<IFileService, FileService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
