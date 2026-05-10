using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NotificationService.Core.Abstractions;
using NotificationService.Core.Abstractions.Repositories;
using NotificationService.Core.Models;
using NotificationService.DataAccess.Configurations;
using NotificationService.DataAccess.Data;
using NotificationService.DataAccess.Options;
using NotificationService.DataAccess.Repositories;

namespace NotificationService.DataAccess.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static void AddDatabaseConfig(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = configuration.GetRequiredSection(nameof(MongoDbOptions)).Get<MongoDbOptions>()!;

            MongoDbConfiguration.Configure();

            services.AddSingleton<IMongoClient>(
                new MongoClient(options.ConnectionString));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IMongoClient>()
                    .GetDatabase(options.DatabaseName));

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IBaseRepository<Notification>, NotificationRepository>();
        }
    }
}
