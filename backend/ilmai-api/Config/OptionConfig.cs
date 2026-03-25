namespace IlmAI.Api.Config;

using IlmAI.Api.Models.Option;

public class OptionConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services.Configure<AppOptions>(config.GetSection("AppOptions"));
        services.Configure<ConnectionStringOptions>(config.GetSection("ConnectionStrings"));
    }
}
