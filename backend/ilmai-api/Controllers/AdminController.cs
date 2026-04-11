namespace IlmAI.Api.Controllers;

using IlmAI.Api.Models.Data;
using IlmAI.Api.Models.Response.Admin;
using IlmAI.Api.Models.Request.Admin;
using IlmAI.Api.Models.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<AdminStatsResponse>> GetStats()
    {
        var oneDayAgo = DateTime.UtcNow.AddDays(-1);

        var totalUsers = await _context.Users.CountAsync();
        var activeUsers24h = await _context.Users
            .CountAsync(u => u.UpdatedAt >= oneDayAgo);

        var totalMessages = await _context.ChatMessages.CountAsync();
        var messages24h = await _context.ChatMessages
            .CountAsync(m => m.CreatedAt >= oneDayAgo);

        var totalSessions = await _context.ChatSessions.CountAsync();

        var topTopics = await _context.ChatSessions
            .GroupBy(s => s.Title)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new TopTopicDto { Title = g.Key, Count = g.Count() })
            .ToListAsync();

        return Ok(new AdminStatsResponse
        {
            TotalUsers = totalUsers,
            ActiveUsers24h = activeUsers24h,
            TotalMessages = totalMessages,
            Messages24h = messages24h,
            TotalSessions = totalSessions,
            TopTopics = topTopics
        });
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<AdminUserResponse>>> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new AdminUserResponse
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FirstName + " " + u.LastName,
                IsEmailVerified = u.IsEmailVerified,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
                LastActiveAt = u.LastActiveAt.HasValue ? DateTime.SpecifyKind(u.LastActiveAt.Value, DateTimeKind.Utc) : null
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("activity-analytics")]
    public async Task<IActionResult> GetActivityAnalytics()
    {
        try
        {
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.UtcNow.Date.AddDays(-i))
                .Reverse()
                .ToList();

            var startDate = last7Days.First();

            // Fetch dates only to optimize DB query
            var messages = await _context.ChatMessages
                .Where(m => m.CreatedAt >= startDate)
                .Select(m => m.CreatedAt.Date)
                .ToListAsync();

            var users = await _context.Users
                .Where(u => u.CreatedAt >= startDate)
                .Select(u => u.CreatedAt.Date)
                .ToListAsync();

            var verifiedCount = await _context.Users.CountAsync(u => u.IsEmailVerified);
            var unverifiedCount = await _context.Users.CountAsync(u => !u.IsEmailVerified);

            var activity = last7Days.Select(date => new
            {
                Date = date.ToString("dd.MM."),
                Messages = messages.Count(d => d == date),
                Users = users.Count(d => d == date)
            }).ToList();

            return Ok(new 
            { 
                activity, 
                distribution = new[] { verifiedCount, unverifiedCount } 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Greška pri učitavanju analitike", details = ex.Message });
        }
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs([FromQuery] string? level = null, [FromQuery] int count = 100)
    {
        var query = _context.SystemLogs.AsQueryable();
        
        if (!string.IsNullOrEmpty(level))
            query = query.Where(l => l.Level == level);

        var logs = await query
            .OrderByDescending(l => l.CreatedAt)
            .Take(count)
            .ToListAsync();

        return Ok(logs);
    }

    [HttpGet("logs/export/{format}")]
    public async Task<IActionResult> ExportLogs(string format)
    {
        var logs = await _context.SystemLogs
            .OrderByDescending(l => l.CreatedAt)
            .Take(1000)
            .ToListAsync();

        var fileName = $"logs_{DateTime.Now:yyyyMMdd_HHmm}.{format.ToLower()}";
        
        if (format.ToLower() == "csv")
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,Timestamp,Level,Message,Source");
            foreach (var log in logs)
                sb.AppendLine($"{log.Id},{log.CreatedAt:yyyy-MM-dd HH:mm:ss},{log.Level},\"{log.Message.Replace("\"", "'")}\",{log.Source}");
            
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
        }
        else
        {
            var sb = new StringBuilder();
            sb.AppendLine("IlmAI SYSTEM LOGS EXPORT");
            sb.AppendLine($"Generated: {DateTime.Now}");
            sb.AppendLine("====================================================");
            foreach (var log in logs)
                sb.AppendLine($"[{log.CreatedAt:yyyy-MM-dd HH:mm:ss}] {log.Level.ToUpper()}: {log.Message} (Source: {log.Source})");
            
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain", fileName);
        }
    }

    [HttpPatch("users/{userId}/toggle-active")]
    public async Task<IActionResult> ToggleUserActive(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        user.IsActive = !user.IsActive;
        await _context.SaveChangesAsync();

        return Ok(new { isActive = user.IsActive });
    }

    [HttpPost("users/{userId}/reset-password")]
    public async Task<IActionResult> ResetPassword(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        // Generate a random 8-character password
        var chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%^&*";
        var random = new Random();
        var newPassword = new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { newPassword });
    }

    [HttpPatch("users/{userId}/role")]
    public async Task<IActionResult> UpdateUserRole(Guid userId, [FromBody] UpdateRoleRequest request)
    {
        var roleName = request.RoleName.ToLower();
        if (roleName != "admin" && roleName != "user")
            return BadRequest(new { error = "Invalid role name. Use 'admin' or 'user'." });

        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return NotFound();

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null) return BadRequest(new { error = "Role not found in database." });

        // Remove all current roles
        _context.UserRoles.RemoveRange(user.UserRoles);

        // Add new role
        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });

        await _context.SaveChangesAsync();

        return Ok(new { roles = new List<string> { role.Name } });
    }
}
