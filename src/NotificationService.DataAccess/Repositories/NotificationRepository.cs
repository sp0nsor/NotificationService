using MongoDB.Driver;
using NotificationService.Core.Models;

namespace NotificationService.DataAccess.Repositories
{
    public sealed class NotificationRepository : BaseRepository<Notification>
    {
        public NotificationRepository(IMongoDatabase database) : base(database) { }
    }
}
