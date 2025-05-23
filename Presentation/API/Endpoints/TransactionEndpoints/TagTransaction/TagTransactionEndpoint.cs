using API.Common.Extensions;
using Application.Features.TransactionFeatures.TagTransaction;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TransactionEndpoints.TagTransaction;

public class TagTransactionEndpoint(Application.Common.Messaging.ICommandHandler<TagTransactionRequest> handler) : Endpoint<TagTransactionRequest>
{
    public override void Configure()
    {
        Post("/transaction/tag");
    }

    public override async Task HandleAsync(TagTransactionRequest req, CancellationToken ct)
    {
        Result result = await handler.Handle(req, ct);

        await this.SendResponse(result);
    }
}
