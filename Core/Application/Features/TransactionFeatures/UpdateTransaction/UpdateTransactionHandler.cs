using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Ardalis.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransactionFeatures.UpdateTransaction;
internal sealed class UpdateTransactionHandler(ITransactionQueryRepository queryRepo, ITransactionCommandRepository commandRepo) : ICommandHandler<UpdateTransactionRequest>
{
    public async Task<Result> Handle(UpdateTransactionRequest command, CancellationToken cancellationToken)
    {
        Transaction? transation = await queryRepo.Transactions
            .Where(t => t.Id == command.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (transation is null)
        {
            return Result.NotFound();
        }

        transation.Update(command.Description, command.Amount, command.Date);

        await commandRepo.UpdateAsync(transation, true, cancellationToken);

        return Result.Success();
    }
}
