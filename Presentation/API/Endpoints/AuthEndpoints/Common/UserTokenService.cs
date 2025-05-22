using System.Security.Claims;
using Application.Common.Messaging;
using Application.Features.AuthFeatures.UserClaim;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Ardalis.Result;
using FastEndpoints;
using FastEndpoints.Security;

namespace API.Endpoints.AuthEndpoints.Common;

public class UserTokenService : RefreshTokenService<TokenRequest, ApplicationTokenResponse>
{
    private readonly IUserRefreshTokenCommandRepository _commandRepo;
    private readonly IUserRefreshTokenQueryRepository _queryRepo;
    private readonly IQueryHandler<UserClaimRequest, List<Claim>> _userClaimHandler;

    public UserTokenService(IConfiguration config, IUserRefreshTokenCommandRepository commandRepo, IUserRefreshTokenQueryRepository queryRepo, IQueryHandler<UserClaimRequest, List<Claim>> userClaimHandler)
    {
        _commandRepo = commandRepo;
        _queryRepo = queryRepo;
        _userClaimHandler = userClaimHandler;

        Setup(x =>
        {
            x.TokenSigningKey = config["Auth:JWTSigningKey"];
            x.AccessTokenValidity = TimeSpan.FromMinutes(config.GetValue<int>("Auth:AccessTokenValidityMinutes"));
            x.RefreshTokenValidity = TimeSpan.FromMinutes(config.GetValue<int>("Auth:RefreshTokenValidityMinutes"));
            x.Endpoint("auth/refresh-token", ep => ep.Summary(s => s.Description = "this is the refresh token endpoint"));
        });
    }

    public override async Task PersistTokenAsync(ApplicationTokenResponse response)
        => await _commandRepo.StoreToken(response.UserId, response.RefreshToken, response.RefreshExpiry);

    public override async Task RefreshRequestValidationAsync(TokenRequest req)
    {
        bool isValidToken = await _queryRepo.IsValidToken(req.UserId, req.RefreshToken);

        if (!isValidToken)
        {
            AddError("The refresh token is not valid!");
        }
    }

    public override async Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
    {
        Result<List<Claim>> result = await _userClaimHandler.Handle(new UserClaimRequest(Convert.ToInt32(request.UserId, System.Globalization.CultureInfo.InvariantCulture)), CancellationToken.None);

        if (result.IsSuccess)
        {
            privileges.Claims.AddRange(result.Value);
        }
    }
}
