using API.Common.Extensions;
using Application.Features.TagFeaturtes.UpdateTag;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TagEndpoints.UpdateTag;

public class UpdateTagEndpoint(Application.Common.Messaging.ICommandHandler<UpdateTagRequest> handler) : Endpoint<UpdateTagRequest>
{
    public override void Configure()
    {
        Patch("/tag/update");
    }

    public override async Task HandleAsync(UpdateTagRequest req, CancellationToken ct)
    {
        Result result = await handler.Handle(req, ct);

        await this.SendResponse(result);
    }
}
