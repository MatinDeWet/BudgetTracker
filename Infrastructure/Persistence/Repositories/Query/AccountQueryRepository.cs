using Application.Common.IdentitySupport;
using Application.Repositories.Query;
using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Query;
internal sealed class AccountQueryRepository : JudgedQueries<BudgetContext>, IAccountQueryRepository
{
    public AccountQueryRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }

    public IQueryable<Account> Accounts => Secure<Account>();

    public IQueryable<Account> InsecureAccounts => _context.Set<Account>();
}
