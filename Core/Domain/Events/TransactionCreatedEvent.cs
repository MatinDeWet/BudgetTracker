using Domain.Common.Messaging;

namespace Domain.Events;
public sealed record TransactionCreatedEvent(Guid AccountId, decimal Amount) : IDomainEvent;
