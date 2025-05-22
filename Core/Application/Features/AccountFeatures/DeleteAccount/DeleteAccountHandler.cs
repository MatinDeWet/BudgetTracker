using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Ardalis.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AccountFeatures.DeleteAccount;
internal sealed class DeleteAccountHandler(IAccountQueryRepository queryRepo, IAccountCommandRepository commandRepo) : ICommandHandler<DeleteAccountRequest>
{
    public async Task<Result> Handle(DeleteAccountRequest command, CancellationToken cancellationToken)
    {
        Account? account = await queryRepo.Accounts
            .Where(a => a.Id == command.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return Result.NotFound();
        }

        await commandRepo.DeleteAsync(account, true, cancellationToken);

        return Result.Success();
    }
}
