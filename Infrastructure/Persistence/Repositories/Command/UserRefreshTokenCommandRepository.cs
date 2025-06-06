﻿using Application.Common.IdentitySupport;
using Application.Repositories.Command;
using Domain.Entities;
using Persistence.Common.Repository;
using Persistence.Data.Context;

namespace Persistence.Repositories.Command;
internal sealed class UserRefreshTokenCommandRepository : JudgedCommands<BudgetContext>, IUserRefreshTokenCommandRepository
{
    public UserRefreshTokenCommandRepository(BudgetContext context, IIdentityInfo info, IEnumerable<IProtected> protection) : base(context, info, protection)
    {
    }

    public async Task StoreToken(string userId, string token, DateTime expirationDate)
    {
        var refreshToken = UserRefreshToken.Create(Convert.ToInt32(userId, System.Globalization.CultureInfo.InvariantCulture), token, expirationDate);

        await InsertAsync(refreshToken, true, CancellationToken.None);
    }
}
