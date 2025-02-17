using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Your_Ride.Helper;
using Your_Ride.Models;
using Your_Ride.Repository.BusRepo;
using Your_Ride.Repository.CollegeRepo;
using Your_Ride.Repository.Generic;
using Your_Ride.Repository.TransactionRepo;
using Your_Ride.Repository.UniversityRepo;
using Your_Ride.Repository.WalletRepo;
using Your_Ride.Services.BusServ;
using Your_Ride.Services.CollegeServ;
using Your_Ride.Services.TransactionServ;
using Your_Ride.Services.UniversityServ;
using Your_Ride.Services.WalletServ;

var builder = WebApplication.CreateBuilder(args);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
})
   .AddEntityFrameworkStores<Context>()
   .AddDefaultTokenProviders();

// Add services to the container.

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<Context>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(typeof(MappingProfile));



builder.Services.AddScoped<IUniversityRepository , UniversityRepository>();
builder.Services.AddScoped<IUniversityService, UniversityService>();

builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();
builder.Services.AddScoped<ICollegeService, CollegeService>();

builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService , WalletService>();

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IBusService, BusService>();



//////////////////////////////////////////////////////////////////
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
