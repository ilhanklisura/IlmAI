namespace IlmAI.Api.Config;

public class MvcConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy =
                    System.Text.Json.JsonNamingPolicy.CamelCase;
            });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
    {
        app.UseCors();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app is WebApplication webApp)
        {
            webApp.MapControllers();
        }
    }
}
