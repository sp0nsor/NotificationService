using NotificationService.Application.Abstractions.DataAccess;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace NotificationService.Api.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var dbInitialize = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitialize.InitializeAsync();
        }

        public static WebApplicationBuilder AddElk(this WebApplicationBuilder app)
        {
            app.Host.UseSerilog((context, config) =>
            {
                var enviroment = context.HostingEnvironment.EnvironmentName;
                var assembly = Assembly.GetExecutingAssembly().GetName().Name;

                config.ReadFrom.Configuration(context.Configuration)
                      .Enrich.FromLogContext()
                      .Enrich.WithProperty("Enviroment", enviroment)
                      .Enrich.WithProperty("Application", assembly)
                      .Enrich.WithProperty("Service", "notification-service")
                      .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                      .WriteTo.Async(a => a.File(
                          path: "/logs/notification-service/log-.json",
                          rollingInterval: RollingInterval.Day,
                          retainedFileCountLimit: 7,
                          fileSizeLimitBytes: 10485760,
                          rollOnFileSizeLimit: true,
                          formatter: new Serilog.Formatting.Json.JsonFormatter(),
                          restrictedToMinimumLevel: LogEventLevel.Information
                      ));
            });

            return app;
        }
    }
}
