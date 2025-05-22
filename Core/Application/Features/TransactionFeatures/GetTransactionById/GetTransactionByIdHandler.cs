using Application.Common.Messaging;
using Application.Repositories.Query;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TransactionFeatures.GetTransactionById;
internal sealed class GetTransactionByIdHandler(ITransactionQueryRepository repo) : IQueryHandler<GetTransactionByIdRequest, GetTransactionByIdResponse>
{
    public async Task<Result<GetTransactionByIdResponse>> Handle(GetTransactionByIdRequest query, CancellationToken cancellationToken)
    {
        GetTransactionByIdResponse? transation = await repo.Transactions
            .Where(t => t.Id == query.Id)
            .Select(t => new GetTransactionByIdResponse
            {
                Id = t.Id,
                AccountId = t.AccountId,
                Direction = t.Direction,
                Description = t.Description,
                Amount = t.Amount,
                Date = t.Date,
                DateCreated = t.DateCreated
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (transation is null)
        {
            return Result.NotFound();
        }

        return transation;
    }
}
