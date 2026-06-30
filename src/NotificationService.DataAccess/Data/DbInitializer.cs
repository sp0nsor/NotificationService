using MongoDB.Driver;
using NotificationService.Application.Abstractions.DataAccess;
using NotificationService.Core.Models;
using NotificationService.DataAccess.Extentions;

namespace NotificationService.DataAccess.Data
{
    internal sealed class DbInitializer : IDbInitializer
    {
        public DbInitializer(
            IMongoDatabase database)
        {
            _database = database;
        }

        private readonly IMongoDatabase _database;

        public async Task InitializeAsync()
        {
            await _database.TryAddCollectionIfNotExistsAsync(nameof(Notification) + 's');
        }
    }
}
