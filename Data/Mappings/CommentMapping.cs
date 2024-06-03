using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api.Domain.Entities;

namespace api.Data.Mappings
{
    public class CommentMapping : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                 .ToTable("Comments");

            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt");

            builder
                .Property(e => e.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder
                .Property(e => e.Title)
                .HasColumnName("Title")
                .IsRequired();

            builder
                .Property(e => e.Content)
                .HasColumnName("Content")
                .IsRequired();

            builder
                .HasOne(e => e.Stock)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.StockId);
        }
    }
}
