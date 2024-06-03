using api.Domain.Requests.Stock;
using Microsoft.AspNetCore.Mvc;
using api.Domain.Interfaces.Services;
using Asp.Versioning;
using api.Domain.Common;
using api.Domain.Responses.Stock;

namespace api.WebApi.Controllers.V1;

/// <summary>
/// Ações de Estoques
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/stock")]
public class StockController(IStockService stockService) : ControllerBase
{
    private readonly IStockService _stockService = stockService;

    /// <summary>
    /// Busca todos os Estoques
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommonResponse<List<StockDto>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CommonResponse<object>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _stockService.GetAllAsync();

        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>
    /// Busca um Estoque específico
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommonResponse<StockDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CommonResponse<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CommonResponse<object>))]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _stockService.GetByIdAsync(id);

        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>
    /// Cria um novo Estoque
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommonResponse<StockDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CommonResponse<object>))]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto request)
    {
        var result = await _stockService.CreateAsync(request);

        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>
    /// Atualiza um Estoque
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommonResponse<StockDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CommonResponse<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CommonResponse<object>))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStockRequestDto request)
    {
        var result = await _stockService.UpdateAsync(id, request);

        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>
    /// Deleta um Estoque
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommonResponse<StockDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CommonResponse<object>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CommonResponse<object>))]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _stockService.DeleteAsync(id);

        return StatusCode((int)result.StatusCode, result);
    }
}