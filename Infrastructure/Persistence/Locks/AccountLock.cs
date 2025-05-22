using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Locks;
internal sealed class AccountLock(BudgetContext context) : Lock<Account>
{
    public override async Task<bool> HasAccess(Account obj, int identityId, RepositoryOperationEnum operation, CancellationToken cancellationToken)
    {
        int userId = obj.UserId;

        if (userId == 0)
        {
            return false;
        }

        return await Task.FromResult(userId == identityId);
    }

    public override IQueryable<Account> Secured(int identityId)
    {
        IQueryable<Account> account =
            from a in context.Set<Account>()
            where a.UserId == identityId
            select a;

        return account;
    }
}
