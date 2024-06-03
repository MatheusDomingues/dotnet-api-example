using api.Domain.Requests.Stock;
using api.Domain.Entities;

namespace api.Domain.Interfaces.Repositories;
public interface IStockRepository
{
    public Task<List<Stock>> GetAllAsync();
    public Task<Stock?> GetByIdAsync(Guid id);
    public Task<Stock> CreateAsync(Stock stockModel);
    public Task<Stock?> UpdateAsync(Guid id, UpdateStockRequestDto stockDto);
    public Task<Stock?> DeleteAsync(Guid id);
}