using Application.Common.IdentitySupport;
using Application.Repositories.Command;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Command;
internal sealed class TransactionTagCommandRepository : JudgedCommands<BudgetContext>, ITransactionCommandRepository
{
    public TransactionTagCommandRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }
}
