using Domain.Enums;

namespace Application.Features.TransactionFeatures.SeachTransaction;
public sealed class SeachTransactionResponse
{
    public Guid Id { get; set; }

    public TransactionDirectionEnum Direction { get; set; }

    public Guid AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}
