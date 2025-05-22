using Application.Common.Messaging;
using Application.Common.Pagination;
using Application.Repositories.Query;
using Ardalis.Result;

namespace Application.Features.TagFeaturtes.SearchTag;
internal sealed class SearchTagHandler(ITagQueryRepository repo) : IQueryHandler<SearchTagRequest, PageableResponse<SearchTagResponse>>
{
    public async Task<Result<PageableResponse<SearchTagResponse>>> Handle(SearchTagRequest query, CancellationToken cancellationToken)
    {
        PageableResponse<SearchTagResponse> tags = await repo.Tags
            .Select(x => new SearchTagResponse
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToPageableListAsync(x => x.Name, OrderDirectionEnum.Ascending, query, cancellationToken);

        return tags;
    }
}
