using Application.Common.IdentitySupport;
using Application.Repositories.Command;
using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Command;
public class UserRefreshTokenCommandRepository : JudgedCommands<BudgetContext>, IUserRefreshTokenCommandRepository
{
    public UserRefreshTokenCommandRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }

    public async Task StoreToken(string userId, string token, DateTime expirationDate)
    {
        var refreshToken = new UserRefreshToken
        {
            Id = Guid.CreateVersion7(),
            UserID = Convert.ToInt32(userId, System.Globalization.CultureInfo.InvariantCulture),
            Token = token,
            ExpiryDate = expirationDate,
        };

        await InsertAsync(refreshToken, true, CancellationToken.None);
    }
}
