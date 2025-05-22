using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Locks;
internal sealed class TransactionLock(BudgetContext context) : Lock<Transaction>
{
    public override async Task<bool> HasAccess(Transaction obj, int identityId, RepositoryOperationEnum operation, CancellationToken cancellationToken)
    {
        Guid accountId = obj.AccountId;

        if (accountId == Guid.Empty)
        {
            return false;
        }

        IQueryable<Account> query =
            from a in context.Set<Account>()
            where
                a.Id == accountId
                && a.UserId == identityId
            select a;

        return await query.AnyAsync(cancellationToken);

    }

    public override IQueryable<Transaction> Secured(int identityId)
    {
        IQueryable<Transaction> transaction =
            from t in context.Set<Transaction>()
            join a in context.Set<Account>() on t.AccountId equals a.Id
            where a.UserId == identityId
            select t;

        return transaction;
    }
}
