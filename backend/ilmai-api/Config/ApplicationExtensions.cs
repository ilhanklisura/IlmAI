namespace Microsoft.AspNetCore.Builder;

using IlmAI.Api.Config;

public static class ApplicationExtensions
{
    public static WebApplicationBuilder AddServiceConfig<T>(this WebApplicationBuilder builder)
        where T : IConfig, new()
    {
        var config = new T();
        config.ConfigureServices(builder.Services, builder.Configuration);
        return builder;
    }

    public static WebApplication AddAppConfig<T>(this WebApplication app) where T : IConfig, new()
    {
        var config = new T();
        config.ConfigureApp(app, app.Environment, app.Configuration);
        return app;
    }
}
