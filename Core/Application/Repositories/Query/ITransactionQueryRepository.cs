using Application.Common.Repository;
using Domain.Entities;

namespace Application.Repositories.Query;
public interface ITransactionQueryRepository : ISecureQuery
{
    IQueryable<Transaction> Transactions { get; }
}
