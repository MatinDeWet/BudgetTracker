using API.Common.Extensions;
using Application.Features.TagFeaturtes.GetTagById;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TagEndpoints.GetTagById;

public class GetTagByIdEndpoint(Application.Common.Messaging.IQueryHandler<GetTagByIdRequest, GetTagByIdResponse> handler) : Endpoint<GetTagByIdRequest, GetTagByIdResponse>
{
    public override void Configure()
    {
        Get("/tag/{Id}");
    }

    public override async Task HandleAsync(GetTagByIdRequest req, CancellationToken ct)
    {
        Result<GetTagByIdResponse> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
