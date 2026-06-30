namespace NotificationService.DataAccess.Options
{
    internal sealed class MongoDbOptions
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}
