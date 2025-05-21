using Application.Common.Repository;

namespace Application.Repositories.Command;
public interface IUserRefreshTokenCommandRepository : ISecureCommand
{
    Task StoreToken(string userId, string token, DateTime expirationDate);
}
