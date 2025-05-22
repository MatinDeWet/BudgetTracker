using Application.Common.IdentitySupport;
using Application.Common.Messaging;
using Application.Repositories.Command;
using Ardalis.Result;
using Domain.Entities;

namespace Application.Features.AccountFeatures.CreateAccount;
internal sealed class CreateAccountHandler(IAccountCommandRepository repo, IIdentityInfo info) : ICommandHandler<CreateAccountRequest, CreateAccountResponse>
{
    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountRequest command, CancellationToken cancellationToken)
    {
        var newAccount = Account.Create(info.GetIdentityId(), command.Name);

        await repo.InsertAsync(newAccount, true, cancellationToken);

        return Result.Created(new CreateAccountResponse(newAccount.Id));
    }
}
