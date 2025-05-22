using API.Common.Extensions;
using Application.Features.TagFeaturtes.DeleteTag;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TagEndpoints.DeleteTag;

public class DeleteTagEndpoint(Application.Common.Messaging.ICommandHandler<DeleteTagRequest> handler) : Endpoint<DeleteTagRequest>
{
    public override void Configure()
    {
        Delete("tag/{Id}");
    }

    public override async Task HandleAsync(DeleteTagRequest req, CancellationToken ct)
    {
        Result result = await handler.Handle(req, ct);

        await this.SendResponse(result);
    }
}
