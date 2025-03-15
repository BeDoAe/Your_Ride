using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

using Microsoft.EntityFrameworkCore;
using Your_Ride.Helper;
using Your_Ride.Models;
using Your_Ride.Repository.AppointmentRepo;
using Your_Ride.Repository.BookRepo;
using Your_Ride.Repository.BusRepo;
using Your_Ride.Repository.CollegeRepo;
using Your_Ride.Repository.ForgetPasswordRepo;
using Your_Ride.Repository.Generic;
using Your_Ride.Repository.TimeRepo;
using Your_Ride.Repository.TransactionRepo;
using Your_Ride.Repository.UniversityRepo;
using Your_Ride.Repository.UserTransactionLogRepo;
using Your_Ride.Repository.WalletRepo;
using Your_Ride.Services.AppointmentServ;
using Your_Ride.Services.BookServ;
using Your_Ride.Services.BusServ;
using Your_Ride.Services.CollegeServ;
using Your_Ride.Services.TimeServ;
using Your_Ride.Services.TransactionServ;
using Your_Ride.Services.UniversityServ;
using Your_Ride.Services.UserTransactionLogServ;
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
})
   .AddEntityFrameworkStores<Context>()
   .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(30); 
    options.SlidingExpiration = true;
});
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

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddScoped<ITimeRepository, TimeRepository>();
builder.Services.AddScoped<ITimeService, TimeService>();


builder.Services.AddScoped<IUserTransactionLogRepository, UserTransactionLogRepository>();
builder.Services.AddScoped<IUserTransactionLogService, UserTransactionLogService>();

// Register Email and SMS services
//builder.Services.AddScoped<Your_Ride.Repository.ForgetPasswordRepo.IEmailSender, EmailSender>();
// Use the Identity Email Sender for password reset
//builder.Services.AddScoped<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, IdentityEmailSender>();
builder.Services.AddScoped<ISmsSender, TwilioSmsSender>();



//////////////////////////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Handles 500 Internal Server Errors
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        // Re-execute the request to the error handling path
        context.Request.Path = "/Home/Error";
        await next();
    }
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
