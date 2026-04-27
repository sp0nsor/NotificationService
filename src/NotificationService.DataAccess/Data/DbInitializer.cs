using MongoDB.Driver;
using NotificationService.Core.Abstractions;
using NotificationService.Core.Models;
using NotificationService.DataAccess.Extentions;

namespace NotificationService.DataAccess.Data
{
    public class DbInitializer : IDbInitializer
    {
        public DbInitializer(
            IMongoDatabase database)
        {
            _database = database;
            data = new NotificationsData();
        }

        private readonly IMongoDatabase _database;
        private readonly NotificationsData data;

        public async Task InitializeAsync()
        {
            await _database.TryAddCollectionIfNotExistsAsync(nameof(Notification) + 's');

            await SeedCollectionAsync(nameof(Notification) + 's', data.Notifications);
        }

        private async Task SeedCollectionAsync<T>(string collectionName, IEnumerable<T> initialData)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var containsData = collection.CountDocuments(FilterDefinition<T>.Empty) > 0;

            if (!containsData)
            {
                await collection.InsertManyAsync(initialData);
            }
        }
    }
}
