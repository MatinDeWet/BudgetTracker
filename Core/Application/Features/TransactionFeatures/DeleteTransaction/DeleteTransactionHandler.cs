using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Ardalis.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransactionFeatures.DeleteTransaction;
internal sealed class DeleteTransactionHandler(ITransactionQueryRepository queryRepo, ITransactionCommandRepository commandRepo) : ICommandHandler<DeleteTransactionRequest>
{
    public async Task<Result> Handle(DeleteTransactionRequest command, CancellationToken cancellationToken)
    {
        Transaction? transation = await queryRepo.Transactions
            .Where(t => t.Id == command.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (transation is null)
        {
            return Result.NotFound();
        }

        transation.Delete();

        await commandRepo.DeleteAsync(transation, true, cancellationToken);

        return Result.Success();
    }
}
