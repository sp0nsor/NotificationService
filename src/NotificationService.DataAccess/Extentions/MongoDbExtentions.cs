using MongoDB.Bson;
using MongoDB.Driver;

namespace NotificationService.DataAccess.Extentions
{
    public static class MongoDbExtentions
    {
        public static async Task<bool> TryAddCollectionIfNotExistsAsync(
            this IMongoDatabase database,
            string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName) ||
                string.IsNullOrWhiteSpace(collectionName))
            {
                return false;
            }

            var filter = new BsonDocument("name", collectionName);
            var collection = await database.ListCollectionNamesAsync(new ListCollectionNamesOptions { Filter = filter });
            var collectionExists = await collection.AnyAsync();

            if (!collectionExists)
            {
                await database.CreateCollectionAsync(collectionName);

                return true;
            }

            return false;
        }
    }
}
