using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public partial class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> entity)
    {
        entity.ToTable(nameof(UserRefreshToken));

        entity.HasKey(x => x.Id);

        entity.HasIndex(x => x.UserID);

        entity.HasIndex(x => x.ExpiryDate)
            .IsDescending(false);

        OnConfigurePartial(entity);
    }
    partial void OnConfigurePartial(EntityTypeBuilder<UserRefreshToken> entity);
}
