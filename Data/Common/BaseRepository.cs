using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using api.Domain.Common;
using api.Data.Context;
using api.Domain.Interfaces;

namespace api.Core.Infrastructure.Common;

public partial class BaseRepository<T>(MyContext context) : IBaseRepository<T> where T : BaseEntity
{
    protected readonly MyContext _context = context ?? throw new ArgumentNullException(nameof(context));
    protected readonly DbSet<T> _dataset = context.Set<T>();
    public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _dataset.SingleOrDefaultAsync(predicate, cancellationToken);
        if (result == null)
            return false;

        _dataset.Remove(result);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<T> itemCollection, CancellationToken cancellationToken = default)
    {
        _dataset.RemoveRange(itemCollection);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _dataset.AnyAsync(predicate, cancellationToken);

    public async Task<T> InsertAsync(T item, CancellationToken cancellationToken)
    {
        if (item.Id == Guid.Empty)
        {
            item.Id = Guid.NewGuid();
        }
        item.CreatedAt = DateTime.UtcNow;

        await _dataset.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return item;
    }

    public async Task<IEnumerable<T>> InsertRangeAsync(IEnumerable<T> item, CancellationToken cancellationToken = default)
    {
        await _dataset.AddRangeAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<T?> SelectAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        await _dataset.AsNoTracking()
                     .SingleOrDefaultAsync(predicate, cancellationToken);

    public async Task<IEnumerable<T>> SelectRangeAsync(Expression<Func<T, bool>> predicate, int startPosition, int numberOfItems, CancellationToken cancellationToken = default) =>
        await _dataset.AsNoTracking()
                   .Where(predicate).Skip(startPosition).Take(numberOfItems)
                   .ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> SelectAllAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        return predicate == null
            ? await _dataset.AsNoTracking()
                           .ToListAsync(cancellationToken)//TODO: ver de retornar IList ou mudar ToList pra outra forma de enemeração
            : await _dataset.AsNoTracking()
                           .Where(predicate)
                           .ToListAsync();
    }

    public async Task<T> UpdateAsync(T item, CancellationToken cancellationToken = default)
    {
        _context.Update(item);
        await _context.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> item, CancellationToken cancellationToken = default)
    {
        _context.UpdateRange(item);
        await _context.SaveChangesAsync(cancellationToken);
        return item;
    }

    #region DisposePattern

    private bool disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
                _context.Dispose();
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}


