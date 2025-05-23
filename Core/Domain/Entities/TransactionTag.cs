namespace Domain.Entities;
public class TransactionTag
{
    public Guid TransactionId { get; private set; }
    public virtual Transaction Transaction { get; private set; } = null!;

    public Guid TagId { get; private set; }
    public virtual Tag Tag { get; private set; } = null!;

    public static TransactionTag Create(Guid transactionId, Guid tagId)
    {
        return new TransactionTag
        {
            TransactionId = transactionId,
            TagId = tagId
        };
    }
}
