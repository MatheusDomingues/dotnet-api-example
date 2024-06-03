using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api.Domain.Entities;

namespace api.Data.Mappings
{
    public class StockMapping : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder
                 .ToTable("Stocks");

            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt");

            builder
                .Property(e => e.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder
                .Property(e => e.Symbol)
                .HasColumnName("Symbol");

            builder
                .Property(e => e.CompanyName)
                .HasColumnName("CompanyName");

            builder
                .Property(e => e.Purchase)
                .HasColumnName("Purchase");

            builder
                .Property(e => e.LastDiv)
                .HasColumnName("LastDiv");

            builder
                .Property(e => e.Industry)
                .HasColumnName("Industry");

            builder
                .Property(e => e.MarketCap)
                .HasColumnName("MarketCap");

            builder
                .HasMany(e => e.Comments)
                .WithOne(e => e.Stock)
                .HasForeignKey(e => e.StockId);
        }
    }
}
