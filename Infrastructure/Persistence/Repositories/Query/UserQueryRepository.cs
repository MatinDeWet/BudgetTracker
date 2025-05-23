using Application.Common.IdentitySupport;
using Application.Repositories.Query;
using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Query;
internal sealed class UserQueryRepository : JudgedQueries<BudgetContext>, IUserQueryRepository
{
    public UserQueryRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }

    public IQueryable<User> Users => Secure<User>();
}
