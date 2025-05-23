using Application.Common.Messaging;
using Application.Repositories.Query;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserFeatures.GetUserById;
internal sealed class GetUserByIdHandler(IUserQueryRepository repo) : IQueryHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdRequest query, CancellationToken cancellationToken)
    {
        GetUserByIdResponse? user = await repo.Users
                    .Where(x => x.Id == query.Id)
                    .Select(x => new GetUserByIdResponse
                    {
                        Id = x.Id,
                        Email = x.IdentityInfo.Email ?? string.Empty,
                    })
                    .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.NotFound();
        }

        return user;
    }
}
