using API.Common.Extensions;
using Application.Features.TransactionFeatures.GetTransactionById;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TransactionEndpoints.GetTransactionById;

public class GetTransactionByIdEndpoint(Application.Common.Messaging.IQueryHandler<GetTransactionByIdRequest, GetTransactionByIdResponse> handler) : Endpoint<GetTransactionByIdRequest, GetTransactionByIdResponse>
{
    public override void Configure()
    {
        Get("/transaction/{Id}");
    }

    public override async Task HandleAsync(GetTransactionByIdRequest req, CancellationToken ct)
    {
        Result<GetTransactionByIdResponse> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
