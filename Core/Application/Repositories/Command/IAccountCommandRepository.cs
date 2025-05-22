using Application.Common.Repository;
using Domain.Entities;

namespace Application.Repositories.Command;
public interface IAccountCommandRepository : ISecureCommand
{
    void InsecureUpdate(Account account);
}
