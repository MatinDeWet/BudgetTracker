using Application.Common.Messaging;
using Application.Common.Pagination;
using Application.Repositories.Query;
using Ardalis.Result;

namespace Application.Features.AccountFeatures.SearchAccount;
internal sealed class SearchAccountHandler(IAccountQueryRepository repo) : IQueryHandler<SearchAccountRequest, PageableResponse<SearchAccountResponse>>
{
    public async Task<Result<PageableResponse<SearchAccountResponse>>> Handle(SearchAccountRequest query, CancellationToken cancellationToken)
    {
        PageableResponse<SearchAccountResponse> accounts = await repo.Accounts
            .Select(x => new SearchAccountResponse
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToPageableListAsync(x => x.Name, OrderDirectionEnum.Ascending, query, cancellationToken);

        return accounts;
    }
}
