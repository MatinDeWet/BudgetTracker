using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public partial class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> entity)
    {
        entity.ToTable(nameof(Transaction));

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(512);

        entity.Property(x => x.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        entity.HasOne(x => x.Account)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        OnConfigurePartial(entity);
    }
    partial void OnConfigurePartial(EntityTypeBuilder<Transaction> entity);
}
