using API.Common.Extensions;
using Application.Features.UserFeatures.GetUserById;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.UserEndpoints.GetUserById;

public class GetUserByIdEndpoint(Application.Common.Messaging.IQueryHandler<GetUserByIdRequest, GetUserByIdResponse> handler) : Endpoint<GetUserByIdRequest, GetUserByIdResponse>
{
    public override void Configure()
    {
        Get("/transaction/{Id}");
    }

    public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
    {
        Result<GetUserByIdResponse> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}

