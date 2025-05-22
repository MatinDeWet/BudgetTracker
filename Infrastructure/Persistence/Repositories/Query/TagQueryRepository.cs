using Application.Common.IdentitySupport;
using Application.Repositories.Query;
using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Query;
public class TagQueryRepository : JudgedQueries<BudgetContext>, ITagQueryRepository
{
    public TagQueryRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }

    public IQueryable<Tag> Tags => Secure<Tag>();
}
