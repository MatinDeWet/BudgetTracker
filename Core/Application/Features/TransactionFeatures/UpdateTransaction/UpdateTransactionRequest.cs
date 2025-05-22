using Domain.Common.Messaging;

namespace Application.Features.TransactionFeatures.UpdateTransaction;
public sealed record UpdateTransactionRequest(Guid Id, string Description, decimal Amount, DateTime Date) : ICommand;
