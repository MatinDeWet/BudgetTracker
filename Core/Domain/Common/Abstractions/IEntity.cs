namespace Domain.Common.Abstractions;
public interface IEntity<T> : IEntity
{
    T Id { get; }
}

public interface IEntity
{
    DateTimeOffset DateCreated { get; }

}
