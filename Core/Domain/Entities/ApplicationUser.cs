using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;
public class ApplicationUser : IdentityUser<int>
{
    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; } = new HashSet<UserRefreshToken>();
}
