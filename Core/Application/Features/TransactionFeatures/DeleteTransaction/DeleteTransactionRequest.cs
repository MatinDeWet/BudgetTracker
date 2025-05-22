using Domain.Common.Messaging;

namespace Application.Features.TransactionFeatures.DeleteTransaction;
public sealed record DeleteTransactionRequest(Guid Id) : ICommand;
