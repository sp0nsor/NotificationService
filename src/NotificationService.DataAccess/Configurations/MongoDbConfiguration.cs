using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using NotificationService.Core.Primitives;

namespace NotificationService.DataAccess.Configurations
{
    public static class MongoDbConfiguration
    {
        public static void Configure()
        {
            BsonSerializer.RegisterSerializer(
                new GuidSerializer(GuidRepresentation.Standard));

            if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
            {
                BsonClassMap.RegisterClassMap<Entity>(cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}
