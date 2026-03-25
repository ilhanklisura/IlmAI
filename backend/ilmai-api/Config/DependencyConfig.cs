namespace IlmAI.Api.Config;

using IlmAI.Api.Services;

public class DependencyConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        // Scrutor: auto-register all IService implementations
        services.Scan(scan => scan.FromAssemblyOf<Program>()
            .AddClasses(classes => classes.AssignableTo<IService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        // Register HttpClient for AI Server communication
        services.AddHttpClient("AIServer", client =>
        {
            var aiHost = config.GetValue<string>("AppOptions:AIServerHost") ?? "http://localhost:5031";
            client.BaseAddress = new Uri(aiHost);
            client.Timeout = TimeSpan.FromSeconds(120);
        });
    }
}
