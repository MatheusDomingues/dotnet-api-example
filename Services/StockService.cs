using System.Net;
using api.Domain.Common;
using api.Domain.Interfaces;
using api.Domain.Interfaces.Repositories;
using api.Domain.Interfaces.Services;
using api.Domain.Requests.Stock;
using api.Domain.Responses.Stock;
using api.Services.Mappers;

namespace api.Services;
public class StockService(IStockRepository stockRepo) : IStockService
{
    private readonly IStockRepository _stockRepo = stockRepo;

    public async Task<ICommonServiceResponse> GetAllAsync()
    {
        var stocks = await _stockRepo.GetAllAsync();
        var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

        return new CommonResponse<List<StockDto>>(stockDto, true, "Estoques encontrados com sucesso", HttpStatusCode.OK);
    }

    public async Task<ICommonServiceResponse> GetByIdAsync(Guid id)
    {
        var stock = await _stockRepo.GetByIdAsync(id);
        if (stock == null)
            return new CommonResponse<object>(new object(), false, "Não foi possível encontrar o estoque", HttpStatusCode.NotFound);

        return new CommonResponse<StockDto>(stock.ToStockDto(), true, "Estoque encontrado com sucesso", HttpStatusCode.OK);
    }

    public async Task<ICommonServiceResponse> CreateAsync(CreateStockRequestDto request)
    {
        var stockModel = request.ToStockFromCreateDto();
        await _stockRepo.CreateAsync(stockModel);

        return new CommonResponse<StockDto>(stockModel.ToStockDto(), true, "Estoque criado com sucesso", HttpStatusCode.Created);
    }

    public async Task<ICommonServiceResponse> UpdateAsync(Guid id, UpdateStockRequestDto request)
    {
        var stockModel = await _stockRepo.UpdateAsync(id, request);
        if (stockModel == null)
            return new CommonResponse<object>(new object(), false, "Não foi possível encontrar o estoque", HttpStatusCode.NotFound);

        return new CommonResponse<StockDto>(stockModel.ToStockDto(), true, "Estoque atualizado com sucesso", HttpStatusCode.OK);
    }

    public async Task<ICommonServiceResponse> DeleteAsync(Guid id)
    {
        var stockModel = await _stockRepo.DeleteAsync(id);
        if (stockModel == null)
            return new CommonResponse<object>(new object(), false, "Não foi possível encontrar o estoque", HttpStatusCode.NotFound);

        return new CommonResponse<StockDto>(stockModel.ToStockDto(), true, "Estoque deletado com sucesso", HttpStatusCode.OK);
    }
}