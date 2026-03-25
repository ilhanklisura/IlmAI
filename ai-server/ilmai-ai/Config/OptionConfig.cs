namespace IlmAI.AI.Config;
using IlmAI.AI.Models.Option;
public class OptionConfig : IConfig
{
    public void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services.Configure<AppOptions>(config);
        services.Configure<OpenAIOptions>(config.GetSection("OpenAI"));
    }
}
