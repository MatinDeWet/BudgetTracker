using API.Common.Extensions;
using Application.Common.Pagination;
using Application.Features.TransactionFeatures.SeachTransaction;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TransactionEndpoints.SearchTransaction;

public class SearchTransactionEndpoint(Application.Common.Messaging.IQueryHandler<SeachTransactionRequest, PageableResponse<SeachTransactionResponse>> handler) : Endpoint<SeachTransactionRequest, PageableResponse<SeachTransactionResponse>>
{
    public override void Configure()
    {
        Post("/transaction/search");
    }

    public override async Task HandleAsync(SeachTransactionRequest req, CancellationToken ct)
    {
        Result<PageableResponse<SeachTransactionResponse>> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
