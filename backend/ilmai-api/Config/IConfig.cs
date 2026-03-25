namespace IlmAI.Api.Config;

public interface IConfig
{
    public void ConfigureServices(
        IServiceCollection services,
        IConfiguration config)
    { }

    public void ConfigureApp(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IConfiguration config)
    { }
}
