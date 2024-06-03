using api.Domain.Common;

namespace api.Domain.Entities;
public class Comment : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid? StockId { get; set; }
    public Stock? Stock { get; set; }
}