namespace IlmAI.AI.Config;
public class MvcConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services.AddControllers().AddJsonOptions(o =>
            o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);
        services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
    }
    public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
    {
        app.UseCors();
        app.UseRouting();
        if (app is WebApplication webApp) webApp.MapControllers();
    }
}
