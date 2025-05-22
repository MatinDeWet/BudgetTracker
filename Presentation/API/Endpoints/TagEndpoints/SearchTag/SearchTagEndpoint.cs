using API.Common.Extensions;
using Application.Common.Pagination;
using Application.Features.TagFeaturtes.SearchTag;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TagEndpoints.SearchTag;

public class SearchTagEndpoint(Application.Common.Messaging.IQueryHandler<SearchTagRequest, PageableResponse<SearchTagResponse>> handler) : Endpoint<SearchTagRequest, SearchTagResponse>
{
    public override void Configure()
    {
        Post("/tag/search");
    }

    public override async Task HandleAsync(SearchTagRequest req, CancellationToken ct)
    {
        Result<PageableResponse<SearchTagResponse>> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
