using Application.Common.Repository;
using Domain.Entities;

namespace Application.Repositories.Query;
public interface IUserRefreshTokenQueryRepository : ISecureQuery
{
    IQueryable<UserRefreshToken> UserRefreshTokens { get; }

    Task<bool> IsValidToken(string userId, string token);
}
