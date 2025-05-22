using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Ardalis.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AccountFeatures.UpdateAccount;
internal sealed class UpdateAccountHandler(IAccountQueryRepository queryRepo, IAccountCommandRepository commandRepo) : ICommandHandler<UpdateAccountRequest>
{
    public async Task<Result> Handle(UpdateAccountRequest command, CancellationToken cancellationToken)
    {
        Account? account = await queryRepo.Accounts
            .Where(a => a.Id == command.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return Result.NotFound();
        }

        account.Update(command.Name);

        await commandRepo.UpdateAsync(account, true, cancellationToken);

        return Result.Success();
    }
}
