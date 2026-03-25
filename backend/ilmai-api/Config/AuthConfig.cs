namespace IlmAI.Api.Config;

using System.Text;
using IlmAI.Api.Models.Option;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public class AuthConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        var appOptions = config.GetSection("AppOptions").Get<AppOptions>()
            ?? throw new InvalidOperationException("Missing AppOptions");

        services.AddHttpContextAccessor();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(appOptions.TokenSecret))
                };
            });

        services.AddAuthorization();
    }

    public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
