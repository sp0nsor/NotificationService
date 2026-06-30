using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NotificationService.Application.Abstractions.DataAccess;
using NotificationService.Core.Primitives;
using System.Linq.Expressions;

namespace NotificationService.DataAccess.Repositories
{
    internal abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        protected BaseRepository(IMongoDatabase database)
        {
            _collection =
                database.GetCollection<TEntity>(typeof(TEntity).Name + "s");
        }

        protected readonly IMongoCollection<TEntity> _collection;

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _collection
                .AsQueryable()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> condition,
            CancellationToken cancellationToken = default)
        {
            return await _collection
                .AsQueryable()
                .Where(condition)
                .ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> condition,
            CancellationToken cancellationToken = default)
        {
            return await _collection
                .AsQueryable()
                .Where(condition)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {

            await _collection.InsertOneAsync(
                entity,
                cancellationToken: cancellationToken);
        }

        public async Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            await _collection.InsertManyAsync(
                entities,
                cancellationToken: cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            await _collection.ReplaceOneAsync(
                e => e.Id == entity.Id,
                entity,
                new ReplaceOptions
                {
                    IsUpsert = false
                },
                cancellationToken);

            return entity;
        }

        public async Task UpdateManyAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            if (!entities.Any())
            {
                return;
            }

            var requests = entities.Select(entity =>
                new ReplaceOneModel<TEntity>(
                    Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                    entity)
                {
                    IsUpsert = false
                });

            await _collection
                .BulkWriteAsync(requests, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(
                e => e.Id == id,
                cancellationToken: cancellationToken);
        }

        public async Task DeleteRangeAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.In(e => e.Id, ids);

            await _collection.DeleteManyAsync(
                filter,
                cancellationToken: cancellationToken);
        }
    }
}
