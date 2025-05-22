using Application.Common.IdentitySupport;
using Application.Repositories.Command;
using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Command;
internal sealed class AccountCommandRepository : JudgedCommands<BudgetContext>, IAccountCommandRepository
{
    public AccountCommandRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }

    public void InsecureUpdate(Account account)
    {
        _context.Update(account);
    }
}
