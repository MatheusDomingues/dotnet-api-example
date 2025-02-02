namespace api.Domain.Responses.Comment;
public class CommentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid? StockId { get; set; }
}