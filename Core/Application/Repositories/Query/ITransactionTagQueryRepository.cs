using Application.Common.Repository;
using Domain.Entities;

namespace Application.Repositories.Query;
public interface ITransactionTagQueryRepository : ISecureQuery
{
    IQueryable<TransactionTag> TransactionTags { get; }
}
