using Application.Common.Messaging;
using Application.Common.Pagination;
using Application.Repositories.Query;
using Ardalis.Result;

namespace Application.Features.TransactionFeatures.SeachTransaction;
internal sealed class SeachTransactionHandler(ITransactionQueryRepository repo) : IQueryHandler<SeachTransactionRequest, PageableResponse<SeachTransactionResponse>>
{
    public async Task<Result<PageableResponse<SeachTransactionResponse>>> Handle(SeachTransactionRequest query, CancellationToken cancellationToken)
    {
        PageableResponse<SeachTransactionResponse> transactions = await repo.Transactions
            .Select(x => new SeachTransactionResponse
            {
                Id = x.Id,
                Direction = x.Direction,
                Description = x.Description,
                Amount = x.Amount,
                Date = x.Date,
            })
            .ToPageableListAsync(x => x.Description, OrderDirectionEnum.Ascending, query, cancellationToken);

        return transactions;
    }
}
