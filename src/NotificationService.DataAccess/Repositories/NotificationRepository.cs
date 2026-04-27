using MongoDB.Driver;
using NotificationService.Core.Models;

namespace NotificationService.DataAccess.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>
    {
        public NotificationRepository(IMongoDatabase database) : base(database) { }
    }
}
