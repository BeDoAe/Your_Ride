using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Your_Ride.Models;

namespace Your_Ride.Helper
{
    public class LockoutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LockoutMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated) // Ensure user is authenticated
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

                        var user = await userManager.FindByIdAsync(userId);

                        if (user != null && user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
                        {
                            // Force logout
                            await signInManager.SignOutAsync();
                            context.User = new ClaimsPrincipal(new ClaimsIdentity()); // Reset user session

                            // Redirect to login page with message
                            context.Response.Redirect("/Account/Login?message=Your account has been locked.");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}