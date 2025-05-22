using Domain.Common.Messaging;

namespace Application.Features.TransactionFeatures.CreateTransaction;
public sealed record CreateTransactionRequest : ICommand<CreateTransactionResponse>
{
    public Guid AccountId { get; set; }

    public string Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}
