using System.Net;
using api.Services.Mappers;
using api.Domain.Common;
using api.Domain.Interfaces.Repositories;
using api.Domain.Interfaces.Services;
using api.Domain.Responses.Comment;

namespace api.Services;
public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepo;
    public CommentService(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }
    public async Task<CommonResponse<List<CommentDto>>> GetAllAsync()
    {
        var comments = await _commentRepo.GetAllAsync();
        var commentDto = comments.Select(c => c.ToCommentDto()).ToList();

        return new(commentDto, true, "Comentários encontrados com sucesso.", HttpStatusCode.OK);
    }
}