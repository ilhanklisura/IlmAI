namespace IlmAI.Api.Services.Default;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IlmAI.Api.Models.Data;
using IlmAI.Api.Models.Data.Entities;
using IlmAI.Api.Models.Option;
using IlmAI.Api.Models.Request.Auth;
using IlmAI.Api.Models.Response.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly AppOptions _options;

    public AuthService(AppDbContext context, IOptions<AppOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password");

        return GenerateToken(user);
    }

    public async Task<TokenResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            throw new InvalidOperationException("Username already exists");

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new InvalidOperationException("Email already exists");

        var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PreferredLanguage = request.PreferredLanguage
        };

        if (userRole != null)
            user.UserRoles.Add(new UserRole { Role = userRole });

        user.Settings = new UserSettings { Language = request.PreferredLanguage };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await _context.Entry(user).Collection(u => u.UserRoles).LoadAsync();
        foreach (var ur in user.UserRoles)
            await _context.Entry(ur).Reference(x => x.Role).LoadAsync();

        return GenerateToken(user);
    }

    public async Task<UserResponse?> GetCurrentUserAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return null;

        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PreferredLanguage = user.PreferredLanguage,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
        };
    }

    private TokenResponse GenerateToken(User user)
    {
        var expires = DateTime.UtcNow.AddMinutes(_options.TokenExpirationMinutes);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.TokenSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email)
        };

        foreach (var ur in user.UserRoles)
            claims.Add(new Claim(ClaimTypes.Role, ur.Role.Name));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires,
            User = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PreferredLanguage = user.PreferredLanguage,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            }
        };
    }
}
