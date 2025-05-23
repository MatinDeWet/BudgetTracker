using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Ardalis.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransactionFeatures.UntagTransaction;
internal sealed class UntagTransactionHandler(ITransactionTagQueryRepository queryRepo, ITransactionTagCommandRepository commandRepo) : ICommandHandler<UntagTransactionRequest>
{
    public async Task<Result> Handle(UntagTransactionRequest command, CancellationToken cancellationToken)
    {
        TransactionTag? transactionTag = await queryRepo.TransactionTags
            .Where(x => x.TagId == command.TagId && x.TransactionId == command.TransactionId)
            .FirstOrDefaultAsync(cancellationToken);

        if (transactionTag is null)
        {
            return Result.NotFound();
        }

        await commandRepo.DeleteAsync(transactionTag, true, cancellationToken);

        return Result.Success();
    }
}
