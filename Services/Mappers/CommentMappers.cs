using api.Domain.Responses.Comment;
using api.Domain.Entities;

namespace api.Services.Mappers;
public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedAt = commentModel.CreatedAt,
            StockId = commentModel.StockId
        };
    }
}