using System.Security.Claims;
using Application.Common.Messaging;
using Ardalis.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.AuthFeatures.UserClaim;
internal sealed class UserClaimHandler(UserManager<ApplicationUser> userManager) : IQueryHandler<UserClaimRequest, List<Claim>>
{
    public async Task<Result<List<Claim>>> Handle(UserClaimRequest query, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await userManager.FindByIdAsync(query.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture));

        if (user is null)
        {
            return Result<List<Claim>>.NotFound();
        }

        IList<Claim> userClaims = await userManager.GetClaimsAsync(user);

        IList<string> roles = await userManager.GetRolesAsync(user);
        IList<Claim> roleClaims = [.. roles.Select(role => new Claim(ClaimTypes.Role, role))];


        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)),
            new (ClaimTypes.Name, user.UserName ?? string.Empty),
            new (ClaimTypes.Email, user.Email ?? string.Empty)
        }
        .Union(roleClaims)
        .Union(userClaims)
        .ToList();

        return claims;
    }
}
