using Domain.Common.Messaging;

namespace Domain.Events;
public sealed record TransactionUpdatedEvent(Guid AccountId, decimal OriginalAmount, decimal NewAmount) : IDomainEvent;
