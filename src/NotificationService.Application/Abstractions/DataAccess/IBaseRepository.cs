using NotificationService.Core.Primitives;
using System.Linq.Expressions;

namespace NotificationService.Application.Abstractions.DataAccess
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}