using System.Security.Claims;
using Domain.Common.Messaging;

namespace Application.Features.AuthFeatures.UserClaim;
public sealed record UserClaimRequest(int UserId) : IQuery<List<Claim>>;
