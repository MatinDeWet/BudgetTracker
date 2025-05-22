using Domain.Common.Abstractions;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities;
public class Transaction : Aggregate<Guid>
{
    public Guid AccountId { get; private set; }
    public virtual Account Account { get; private set; } = null!;

    public TransactionDirectionEnum Direction { get; private set; }

    public string Description { get; private set; } = null!;

    public decimal Amount { get; private set; }

    public DateTime Date { get; private set; }


    public static Transaction Create(Guid accountId, string description, decimal amount, DateTime date)
    {
        var transation = new Transaction
        {
            Id = Guid.CreateVersion7(),
            AccountId = accountId,
            Direction = amount >= 0 ? TransactionDirectionEnum.Income : TransactionDirectionEnum.Expense,
            Description = description,
            Amount = amount,
            Date = date
        };

        transation.AddDomainEvent(new TransactionCreatedEvent(accountId, amount));

        return transation;
    }

    public void Update(string description, decimal amount, DateTime date)
    {
        AddDomainEvent(new TransactionUpdatedEvent(AccountId, Amount, amount));

        Description = description;
        Amount = amount;
        Direction = amount >= 0 ? TransactionDirectionEnum.Income : TransactionDirectionEnum.Expense;
        Date = date;
    }
}
