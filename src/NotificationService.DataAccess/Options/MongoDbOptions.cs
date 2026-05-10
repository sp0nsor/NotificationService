namespace NotificationService.DataAccess.Options
{
    public class MongoDbOptions
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}
