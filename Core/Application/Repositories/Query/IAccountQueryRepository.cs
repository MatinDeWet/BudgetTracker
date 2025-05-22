using Application.Common.Repository;
using Domain.Entities;

namespace Application.Repositories.Query;
public interface IAccountQueryRepository : ISecureQuery
{
    IQueryable<Account> Accounts { get; }

    IQueryable<Account> InsecureAccounts { get; }
}
