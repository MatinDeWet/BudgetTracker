namespace Domain.Common.Abstractions;
public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; } = default!;
    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;
}
