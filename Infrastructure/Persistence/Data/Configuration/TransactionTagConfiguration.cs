using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public partial class TransactionTagConfiguration : IEntityTypeConfiguration<TransactionTag>
{
    public void Configure(EntityTypeBuilder<TransactionTag> entity)
    {
        entity.ToTable(nameof(TransactionTag));

        entity.HasKey(x => new { x.TransactionId, x.TagId });

        entity.HasOne(x => x.Transaction)
            .WithMany(x => x.Tags)
            .HasForeignKey(x => x.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.Tag)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        OnConfigurePartial(entity);
    }
    partial void OnConfigurePartial(EntityTypeBuilder<TransactionTag> entity);
}
