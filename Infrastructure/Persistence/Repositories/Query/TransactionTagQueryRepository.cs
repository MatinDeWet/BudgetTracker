using Application.Common.IdentitySupport;
using Application.Repositories.Query;
using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Query;
internal sealed class TransactionTagQueryRepository : JudgedQueries<BudgetContext>, ITransactionTagQueryRepository
{
    public TransactionTagQueryRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }

    public IQueryable<TransactionTag> TransactionTags => Secure<TransactionTag>();
}
