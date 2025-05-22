using Application.Common.Messaging;
using Application.Repositories.Query;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AccountFeatures.GetAccountById;
internal sealed class GetAccountByIdHandler(IAccountQueryRepository repo) : IQueryHandler<GetAccountByIdRequest, GetAccountByIdResponse>
{
    public async Task<Result<GetAccountByIdResponse>> Handle(GetAccountByIdRequest query, CancellationToken cancellationToken)
    {
        GetAccountByIdResponse? account = await repo.Accounts
            .Where(x => x.Id == query.Id)
            .Select(x => new GetAccountByIdResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                Name = x.Name,
                DateCreated = x.DateCreated
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return Result.NotFound();
        }

        return account;
    }
}
