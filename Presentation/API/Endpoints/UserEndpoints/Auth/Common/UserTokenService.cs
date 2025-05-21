using Application.Repositories.Command;
using Application.Repositories.Query;
using FastEndpoints;
using FastEndpoints.Security;

namespace API.Endpoints.UserEndpoints.Auth.Common;

public class UserTokenService : RefreshTokenService<TokenRequest, ApplicationTokenResponse>
{
    private readonly IUserRefreshTokenCommandRepository _commandRepo;
    private readonly IUserRefreshTokenQueryRepository _queryRepo;

    public UserTokenService(IConfiguration config, IUserRefreshTokenCommandRepository commandRepo, IUserRefreshTokenQueryRepository queryRepo)
    {
        _commandRepo = commandRepo;
        _queryRepo = queryRepo;

        Setup(x =>
        {
            x.TokenSigningKey = config["JWTSigningKey"];
            x.AccessTokenValidity = TimeSpan.FromMinutes(config.GetValue<int>("Auth:AccessTokenValidityMinutes"));
            x.RefreshTokenValidity = TimeSpan.FromMinutes(config.GetValue<int>("Auth:RefreshTokenValidityMinutes"));
            x.Endpoint("/user/auth/refresh-token", ep => ep.Summary(s => s.Description = "this is the refresh token endpoint"));
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

    public override Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
    {
        return Task.CompletedTask;
    }
}
