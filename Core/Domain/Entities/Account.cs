using Domain.Common.Abstractions;

namespace Domain.Entities;
public class Account : Entity<Guid>
{
    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;

    public decimal Balance { get; private set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();

    public static Account Create(int userId, string name)
    {
        return new Account
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            Name = name
        };
    }

    public void Update(string name)
    {
        Name = name;
    }
}
