using Domain.Enums;

namespace Application.Features.TransactionFeatures.GetTransactionById;
public sealed record GetTransactionByIdResponse
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public TransactionDirectionEnum Direction { get; set; }

    public string Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public DateTimeOffset DateCreated { get; set; }
}
