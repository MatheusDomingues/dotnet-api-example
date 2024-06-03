using Microsoft.AspNetCore.Mvc;
using api.Domain.Interfaces.Services;
using Asp.Versioning;
using api.Domain.Common;
using api.Domain.Responses.Comment;

namespace api.WebApi.Controllers.V1;

/// <summary>
/// Ações de Comentários
/// </summary>
/// <param name="commentService"></param>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/comment")]
public class CommentController(ICommentService commentService) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;

    /// <summary>
    /// Busca todos os Comentários
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommonResponse<List<CommentDto>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CommonResponse<object>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _commentService.GetAllAsync();

        return StatusCode((int)result.StatusCode, result);
    }
}