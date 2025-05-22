using Domain.Common.Messaging;

namespace Domain.Events;
public sealed record class TransactionDeletedEvent(Guid AccountId, decimal Amount) : IDomainEvent;
