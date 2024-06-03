using api.Domain.Common;
using api.Domain.Responses.Comment;

namespace api.Domain.Interfaces.Services;
public interface ICommentService
{
    Task<CommonResponse<List<CommentDto>>> GetAllAsync();
}