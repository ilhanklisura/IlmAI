namespace IlmAI.AI.Config;
using IlmAI.AI.Services;
public class DependencyConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services.Scan(scan => scan.FromAssemblyOf<Program>()
            .AddClasses(classes => classes.AssignableTo<IService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}
