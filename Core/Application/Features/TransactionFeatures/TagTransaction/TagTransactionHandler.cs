using Application.Common.Messaging;
using Application.Repositories.Command;
using Ardalis.Result;
using Domain.Entities;

namespace Application.Features.TransactionFeatures.TagTransaction;
internal sealed class TagTransactionHandler(ITransactionTagCommandRepository repo) : ICommandHandler<TagTransactionRequest>
{
    public async Task<Result> Handle(TagTransactionRequest command, CancellationToken cancellationToken)
    {
        var transactionTagLink = TransactionTag.Create(command.TransactionId, command.TagId);

        await repo.InsertAsync(transactionTagLink, true, cancellationToken);

        return Result.Success();
    }
}
