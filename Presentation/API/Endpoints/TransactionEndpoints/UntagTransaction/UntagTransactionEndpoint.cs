using API.Common.Extensions;
using Application.Features.TransactionFeatures.UntagTransaction;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TransactionEndpoints.UntagTransaction;

public class UntagTransactionEndpoint(Application.Common.Messaging.ICommandHandler<UntagTransactionRequest> handler) : Endpoint<UntagTransactionRequest>
{
    public override void Configure()
    {
        Post("/transaction/untag");
    }

    public override async Task HandleAsync(UntagTransactionRequest req, CancellationToken ct)
    {
        Result result = await handler.Handle(req, ct);

        await this.SendResponse(result);
    }
}
