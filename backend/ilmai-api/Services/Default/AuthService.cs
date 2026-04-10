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
using IlmAI.Api.Services.Interfaces;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly AppOptions _options;
    private readonly IEmailService _emailService;

    public AuthService(
        AppDbContext context, 
        IOptions<AppOptions> options,
        IEmailService emailService)
    {
        _context = context;
        _options = options.Value;
        _emailService = emailService;
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Username);

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

        // Email Verification Logic
        var verificationCode = new Random().Next(100000, 999999).ToString();
        user.EmailVerificationCode = verificationCode;
        user.IsEmailVerified = false;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // "Send" email (Mock or real)
        await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);

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
            IsEmailVerified = user.IsEmailVerified,
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
                IsEmailVerified = user.IsEmailVerified,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            }
        };
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            throw new InvalidOperationException("New password and confirmation do not match");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Current password is incorrect");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task VerifyEmailAsync(string email, string code)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (user.IsEmailVerified)
            return;

        if (user.EmailVerificationCode != code)
            throw new InvalidOperationException("Invalid verification code");

        user.IsEmailVerified = true;
        user.EmailVerificationCode = null;
        user.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
    }

    public async Task ResendVerificationCodeAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (user.IsEmailVerified)
            throw new InvalidOperationException("Email is already verified");

        // Generate new 6-digit code
        user.EmailVerificationCode = new Random().Next(100000, 999999).ToString();
        user.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();

        // Send email (Mock or real)
        await _emailService.SendVerificationEmailAsync(user.Email, user.EmailVerificationCode);
    }
}
