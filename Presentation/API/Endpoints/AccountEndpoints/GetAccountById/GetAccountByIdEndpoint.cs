using API.Common.Extensions;
using Application.Features.AccountFeatures.GetAccountById;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.AccountEndpoints.GetAccountById;

public class GetAccountByIdEndpoint(Application.Common.Messaging.IQueryHandler<GetAccountByIdRequest, GetAccountByIdResponse> handler) : Endpoint<GetAccountByIdRequest, GetAccountByIdResponse>
{
    public override void Configure()
    {
        Get("/account/{Id}");
    }

    public override async Task HandleAsync(GetAccountByIdRequest req, CancellationToken ct)
    {
        Result<GetAccountByIdResponse> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
