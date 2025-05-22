using API.Common.Extensions;
using Application.Common.Pagination;
using Application.Features.AccountFeatures.SearchAccount;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.AccountEndpoints.SearchAccount;

public class SearchAccountEndpoint(Application.Common.Messaging.IQueryHandler<SearchAccountRequest, PageableResponse<SearchAccountResponse>> handler) : Endpoint<SearchAccountRequest, PageableResponse<SearchAccountResponse>>
{
    public override void Configure()
    {
        Post("/account/search");
    }

    public override async Task HandleAsync(SearchAccountRequest req, CancellationToken ct)
    {
        Result<PageableResponse<SearchAccountResponse>> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
