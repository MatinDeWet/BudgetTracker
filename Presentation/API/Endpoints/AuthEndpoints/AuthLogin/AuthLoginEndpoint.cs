using System.Security.Claims;
using API.Common.Extensions;
using API.Endpoints.AuthEndpoints.Common;
using Application.Common.Messaging;
using Application.Features.AuthFeatures.UserClaim;
using Ardalis.Result;
using Domain.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace API.Endpoints.AuthEndpoints.AuthLogin;

public class AuthLoginEndpoint(UserManager<ApplicationUser> userManager, IQueryHandler<UserClaimRequest, List<Claim>> userClaimHandler) : Endpoint<AuthLoginRequest, ApplicationTokenResponse>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AuthLoginRequest req, CancellationToken ct)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(req.Email);

        if (user is null)
        {
            ThrowError("Invalid Email / Password");
        }

        bool isValidPassword = await userManager.CheckPasswordAsync(user, req.Password);

        if (!isValidPassword)
        {
            ThrowError("Invalid Email / Password");
        }

        Result<List<Claim>> result = await userClaimHandler.Handle(new UserClaimRequest(user.Id), ct);

        await this.SendResponseAsync(result, async claims => await CreateTokenWith<UserTokenService>(user.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), options => options.Claims.AddRange(claims.Value)));
    }
}
