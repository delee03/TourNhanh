using System;
using Microsoft.EntityFrameworkCore;
using TourNhanh.Models;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using Microsoft.Extensions.Options;
using TourNhanh.DataAcess;

using TourNhanh.Repositories.Implementations;
using TourNhanh.Repositories.Interfaces;

internal class Program



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITransportRepository, TransportRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ITourImage, TourImageRepository>();
builder.Services.AddScoped<ITourDetail, TourDetailRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
      
        var configuration = builder.Configuration;

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        /*builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();*/
      

        //Phan quyen user admin
        builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddDefaultTokenProviders()
        .AddDefaultUI()
        .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        
        //login với FACEBOOk
        builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = "1091659662059239";
            facebookOptions.AppSecret = "447850362b54bdbf99d4979506a719a0";
           
        });

        //login với Google
        builder.Services.AddAuthentication().AddGoogle(ggOptions =>
        {
            ggOptions.ClientId = configuration["Authentication:Google:ClientId"];
            ggOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];

        });



        var app = builder.Build();


        /*builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AppDbContext>();*/


        
        // Add services to the container.
      

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

        app.UseSession();

        app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
        );
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}