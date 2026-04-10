namespace IlmAI.Api.Services;

using IlmAI.Api.Models.Request.Auth;
using IlmAI.Api.Models.Response.Auth;

public interface IAuthService : IService
{
    Task<TokenResponse> LoginAsync(LoginRequest request);
    Task<TokenResponse> RegisterAsync(RegisterRequest request);
    Task<UserResponse?> GetCurrentUserAsync(Guid userId);
    Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
    Task VerifyEmailAsync(string email, string code);
    Task ResendVerificationCodeAsync(string email);
}
