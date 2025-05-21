namespace Domain.Common.Abstractions;
public interface IEntity<T>
{
    T Id { get; set; }

    DateTimeOffset DateCreated { get; set; }
}
