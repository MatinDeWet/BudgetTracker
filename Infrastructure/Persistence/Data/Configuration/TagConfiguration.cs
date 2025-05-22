using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public partial class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> entity)
    {
        entity.ToTable(nameof(Tag));

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(64);

        entity.HasOne(x => x.User)
            .WithMany(x => x.Tags)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        OnConfigurePartial(entity);
    }
    partial void OnConfigurePartial(EntityTypeBuilder<Tag> entity);
}
