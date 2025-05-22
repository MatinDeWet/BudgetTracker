using API.Common.Extensions;
using Application.Features.TagFeaturtes.CreateTag;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TagEndpoints.CreateTag;

public class CreateTagEndpoint(Application.Common.Messaging.ICommandHandler<CreateTagRequest, CreateTagResponse> handler) : Endpoint<CreateTagRequest>
{
    public override void Configure()
    {
        Post("/tag/create");
    }

    public override async Task HandleAsync(CreateTagRequest req, CancellationToken ct)
    {
        Result<CreateTagResponse> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
