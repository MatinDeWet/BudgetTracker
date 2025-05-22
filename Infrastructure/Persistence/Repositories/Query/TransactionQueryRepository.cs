using Application.Common.IdentitySupport;
using Application.Repositories.Query;
using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Query;
internal sealed class TransactionQueryRepository : JudgedQueries<BudgetContext>, ITransactionQueryRepository
{
    public TransactionQueryRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }
    public IQueryable<Transaction> Transactions => Secure<Transaction>();
}
