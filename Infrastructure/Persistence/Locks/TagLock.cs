using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Locks;
internal sealed class TagLock(BudgetContext context) : Lock<Tag>
{
    public override async Task<bool> HasAccess(Tag obj, int identityId, RepositoryOperationEnum operation, CancellationToken cancellationToken)
    {
        int userId = obj.UserId;

        if (userId == 0)
        {
            return false;
        }

        return await Task.FromResult(userId == identityId);
    }

    public override IQueryable<Tag> Secured(int identityId)
    {
        IQueryable<Tag> tag =
            from t in context.Set<Tag>()
            where t.UserId == identityId
            select t;

        return tag;
    }
}
