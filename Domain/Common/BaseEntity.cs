using System.ComponentModel.DataAnnotations;

namespace api.Domain.Common
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public void SetUpdateAt()
        => UpdatedAt = DateTime.UtcNow;
    }
}