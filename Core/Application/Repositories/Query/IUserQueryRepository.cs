using Application.Common.Repository;
using Domain.Entities;

namespace Application.Repositories.Query;
public interface IUserQueryRepository : ISecureQuery
{
    IQueryable<User> Users { get; }
}
