using Domain.Common.Messaging;

namespace Persistence.Common.DomainEvents;
public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
