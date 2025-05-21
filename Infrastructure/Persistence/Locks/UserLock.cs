using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Locks;
public class UserLock(BudgetContext context) : Lock<User>
{
    public override async Task<bool> HasAccess(User obj, int identityId, RepositoryOperationEnum operation, CancellationToken cancellationToken)
    {
        if (operation == RepositoryOperationEnum.Insert)
        {
            return await Task.FromResult(true);
        }

        int userId = obj.Id;

        if (userId == 0)
        {
            return false;
        }

        return userId == identityId;
    }

    public override IQueryable<User> Secured(int identityId)
    {
        IQueryable<User> user =
            from u in context.Set<User>()
            where u.Id == identityId
            select u;

        return user;
    }
}
