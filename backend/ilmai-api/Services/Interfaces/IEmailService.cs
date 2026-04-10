namespace IlmAI.Api.Services.Interfaces;

public interface IEmailService : IService
{
    Task SendVerificationEmailAsync(string email, string code);
}
