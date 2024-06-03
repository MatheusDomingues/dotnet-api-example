using System.Linq.Expressions;
using api.Domain.Common;

namespace api.Domain.Interfaces;

public interface IBaseRepository<T> : IDisposable where T : BaseEntity
{
    Task<T> InsertAsync(T item, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> itemCollection, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T item, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> item, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> DeleteRangeAsync(IEnumerable<T> itemCollection, CancellationToken cancellationToken = default);
    Task<T?> SelectAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> SelectRangeAsync(Expression<Func<T, bool>> predicate, int startPosition, int numberOfItems, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> SelectAllAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<bool> ExistAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}
