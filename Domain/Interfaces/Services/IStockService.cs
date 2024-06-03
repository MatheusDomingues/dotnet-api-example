using api.Domain.Common;
using api.Domain.Entities;
using api.Domain.Requests.Stock;
using api.Domain.Responses.Stock;

namespace api.Domain.Interfaces.Services;
public interface IStockService
{
    public Task<ICommonServiceResponse> GetAllAsync();
    public Task<ICommonServiceResponse> GetByIdAsync(Guid id);
    public Task<ICommonServiceResponse> CreateAsync(CreateStockRequestDto request);
    public Task<ICommonServiceResponse> UpdateAsync(Guid id, UpdateStockRequestDto request);
    public Task<ICommonServiceResponse> DeleteAsync(Guid id);
}