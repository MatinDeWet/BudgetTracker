using Domain.Common.Abstractions;

namespace Domain.Entities;
public class User : Entity<int>
{
    public virtual ApplicationUser IdentityInfo { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
}
