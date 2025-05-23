using Domain.Common.Messaging;

namespace Application.Features.TransactionFeatures.UntagTransaction;
public sealed record UntagTransactionRequest(Guid TagId, Guid TransactionId) : ICommand;
