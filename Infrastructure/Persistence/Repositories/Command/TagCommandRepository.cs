using Application.Common.IdentitySupport;
using Application.Repositories.Command;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Command;
public class TagCommandRepository : JudgedCommands<BudgetContext>, ITagCommandRepository
{
    public TagCommandRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }
}
