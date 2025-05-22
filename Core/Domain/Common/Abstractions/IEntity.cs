namespace Domain.Common.Abstractions;
public interface IEntity<T>
{
    T Id { get; }

    DateTimeOffset DateCreated { get; }
}
