using System.Security.Claims;
using IlmAI.Api.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace IlmAI.Api.Middleware;

public class UserPresenceMiddleware
{
    private readonly RequestDelegate _next;

    public UserPresenceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                // Detailed debug info
                var user = await dbContext.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == userId);
                
                if (user != null)
                {
                    Console.WriteLine($"[Middleware] SUCCESS: Found user {user.Username} ({userId}). Updating presence...");
                    user.LastActiveAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync();
                }
                else 
                {
                    Console.WriteLine($"[Middleware] ERROR: Authenticated User ID {userId} NOT FOUND in database!");
                }
            }
            else 
            {
               Console.WriteLine($"[Middleware] WARNING: Authenticated but NameIdentifier claim is missing or invalid: {userIdClaim?.Value}");
            }
        }

        await _next(context);
    }
}
