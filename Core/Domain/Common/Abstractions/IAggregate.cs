using Domain.Common.Messaging;

namespace Domain.Common.Abstractions;
public interface IAggregate<T> : IAggregate, IEntity<T>
{
}

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();

    void AddDomainEvent(IDomainEvent domainEvent);
}
