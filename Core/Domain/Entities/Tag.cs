using Domain.Common.Abstractions;

namespace Domain.Entities;
public class Tag : Entity<Guid>
{
    public int UserId { get; private set; }
    public virtual User User { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public static Tag Create(int userId, string name)
    {
        return new Tag
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            Name = name
        };
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
