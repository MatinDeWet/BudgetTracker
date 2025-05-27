using Application.Common.Messaging;
using Application.Common.Pagination;
using Application.Repositories.Query;
using Ardalis.Result;
using Domain.Entities;

namespace Application.Features.TransactionFeatures.SeachTransaction;
internal sealed class SeachTransactionHandler(ITransactionQueryRepository repo) : IQueryHandler<SeachTransactionRequest, PageableResponse<SeachTransactionResponse>>
{
    public async Task<Result<PageableResponse<SeachTransactionResponse>>> Handle(SeachTransactionRequest query, CancellationToken cancellationToken)
    {
        IQueryable<Transaction> transationQuery = repo.Transactions;


        if (query.AccountId is not null)
        {
            transationQuery = transationQuery
                .Where(x => x.AccountId == query.AccountId);
        }

        if (query.TransactionDirection is not null)
        {
            transationQuery = transationQuery
                .Where(x => x.Direction == query.TransactionDirection);
        }

        if (query.DateRange is not null)
        {
            if (query.DateRange.StartDate is not null)
            {
                transationQuery = transationQuery
                    .Where(x => x.Date >= query.DateRange.StartDate.Value.ToDateTime(TimeOnly.MinValue));
            }

            if (query.DateRange.EndDate is not null)
            {
                transationQuery = transationQuery
                    .Where(x => x.Date <= query.DateRange.EndDate.Value.ToDateTime(TimeOnly.MaxValue));
            }
        }

        PageableResponse<SeachTransactionResponse> transactions = await transationQuery
            .Select(x => new SeachTransactionResponse
            {
                Id = x.Id,
                Direction = x.Direction,
                Description = x.Description,
                AccountId = x.AccountId,
                AccountName = x.Account.Name,
                Amount = x.Amount,
                Date = x.Date,
            })
            .ToPageableListAsync(x => x.Description, OrderDirectionEnum.Ascending, query, cancellationToken);

        return transactions;
    }
}
