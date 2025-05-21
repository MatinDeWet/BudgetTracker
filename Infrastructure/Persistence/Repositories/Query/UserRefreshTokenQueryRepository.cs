using Application.Common.IdentitySupport;
using Application.Repositories.Query;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Query;
public class UserRefreshTokenQueryRepository : JudgedQueries<BudgetContext>, IUserRefreshTokenQueryRepository
{

    public UserRefreshTokenQueryRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }

    public IQueryable<UserRefreshToken> UserRefreshTokens => _context.Set<UserRefreshToken>();

    public async Task<bool> IsValidToken(string userId, string token)
    {
        bool isValid = await UserRefreshTokens
            .Where(x => x.UserID == Convert.ToInt32(userId, System.Globalization.CultureInfo.InvariantCulture)
                && x.Token == token
                && x.ExpiryDate >= DateTime.Now
                )
            .AnyAsync();

        return isValid;
    }
}
