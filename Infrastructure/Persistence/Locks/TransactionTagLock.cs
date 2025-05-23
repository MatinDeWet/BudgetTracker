using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Locks;
internal sealed class TransactionTagLock(BudgetContext context) : Lock<TransactionTag>
{
    public override async Task<bool> HasAccess(TransactionTag obj, int identityId, RepositoryOperationEnum operation, CancellationToken cancellationToken)
    {
        Guid tagId = obj.TagId;
        Guid transactionId = obj.TransactionId;

        bool hasTransactionAccess =
            await (from t in context.Set<Transaction>()
                   join a in context.Set<Account>() on t.AccountId equals a.Id
                   where t.Id == transactionId && a.UserId == identityId
                   select t)
                .AnyAsync(cancellationToken);

        bool hasTagAccess =
            await (from t in context.Set<Tag>()
                   where t.Id == tagId && t.UserId == identityId
                   select t)
                .AnyAsync(cancellationToken);

        return hasTransactionAccess && hasTagAccess;
    }

    public override IQueryable<TransactionTag> Secured(int identityId)
    {
        IQueryable<TransactionTag> transactionTag =
            from tt in context.Set<TransactionTag>()
            join t in context.Set<Transaction>() on tt.TransactionId equals t.Id
            join a in context.Set<Account>() on t.AccountId equals a.Id
            join tag in context.Set<Tag>() on tt.TagId equals tag.Id
            where a.UserId == identityId && tag.UserId == identityId
            select tt;

        return transactionTag;
    }
}
