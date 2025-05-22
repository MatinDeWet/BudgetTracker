namespace Domain.Common.Abstractions;
public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; protected set; } = default!;
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.UtcNow;
}
