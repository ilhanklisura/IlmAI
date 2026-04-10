namespace IlmAI.Api.Services.Default;

using System.Net.Mail;
using IlmAI.Api.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendVerificationEmailAsync(string email, string code)
    {
        var smtpHost = _configuration["SMTP_HOST"];
        
        if (string.IsNullOrEmpty(smtpHost))
        {
            // MOCK MODE: Just log to console
            _logger.LogWarning("SMTP_HOST not configured. MOCK EMAIL SENT to {Email}: Code is {Code}", email, code);
            Console.WriteLine("====================================================");
            Console.WriteLine($"EMAIL MOCK: To: {email}");
            Console.WriteLine($"Subject: Vaš IlmAI verifikacijski kod");
            Console.WriteLine($"Body: Vaš kod je: {code}");
            Console.WriteLine("====================================================");
            return;
        }

        // PRODUCTION MODE: Real Gmail/SMTP
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("IlmAI Platform", _configuration["SMTP_USER"]));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "IlmAI - Verifikacija email adrese";

            message.Body = new TextPart("plain")
            {
                Text = $"Dobrodošli na IlmAI!\n\nVaš verifikacijski kod je: {code}\n\nUnesite ovaj kod u aplikaciju kako biste završili registraciju."
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(smtpHost, int.Parse(_configuration["SMTP_PORT"] ?? "587"), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_configuration["SMTP_USER"], _configuration["SMTP_PASS"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
            _logger.LogInformation("Email successfully sent to {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", email);
            throw;
        }
    }
}
