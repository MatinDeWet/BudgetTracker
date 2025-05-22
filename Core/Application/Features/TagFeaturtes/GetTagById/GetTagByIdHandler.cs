using Application.Common.Messaging;
using Application.Repositories.Query;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TagFeaturtes.GetTagById;
internal sealed class GetTagByIdHandler(ITagQueryRepository repo) : IQueryHandler<GetTagByIdRequest, GetTagByIdResponse>
{
    public async Task<Result<GetTagByIdResponse>> Handle(GetTagByIdRequest query, CancellationToken cancellationToken)
    {
        GetTagByIdResponse? tag = await repo.Tags
            .Where(x => x.Id == query.Id)
            .Select(x => new GetTagByIdResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                Name = x.Name,
                DateCreated = x.DateCreated
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (tag is null)
        {
            return Result.NotFound();
        }

        return tag;
    }
}
