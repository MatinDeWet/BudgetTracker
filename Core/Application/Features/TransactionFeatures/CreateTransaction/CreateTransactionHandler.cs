using Application.Common.Messaging;
using Application.Repositories.Command;
using Ardalis.Result;
using Domain.Entities;

namespace Application.Features.TransactionFeatures.CreateTransaction;
internal sealed class CreateTransactionHandler(ITransactionCommandRepository repo) : ICommandHandler<CreateTransactionRequest, CreateTransactionResponse>
{
    public async Task<Result<CreateTransactionResponse>> Handle(CreateTransactionRequest command, CancellationToken cancellationToken)
    {
        var newTransaction = Transaction.Create(command.AccountId, command.Description, command.Amount, command.Date);

        await repo.InsertAsync(newTransaction, true, cancellationToken);

        return Result.Created(new CreateTransactionResponse(newTransaction.Id));
    }
}
