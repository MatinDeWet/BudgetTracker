using Domain.Common.Messaging;

namespace Application.Common.Messaging;
public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken);
}
