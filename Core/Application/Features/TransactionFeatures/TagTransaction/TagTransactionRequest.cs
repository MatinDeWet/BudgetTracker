using Domain.Common.Messaging;

namespace Application.Features.TransactionFeatures.TagTransaction;
public sealed record TagTransactionRequest(Guid TagId, Guid TransactionId) : ICommand;
