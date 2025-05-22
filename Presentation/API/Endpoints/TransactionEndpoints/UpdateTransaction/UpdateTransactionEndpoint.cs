using API.Common.Extensions;
using Application.Features.TransactionFeatures.UpdateTransaction;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TransactionEndpoints.UpdateTransaction;

public class UpdateTransactionEndpoint(Application.Common.Messaging.ICommandHandler<UpdateTransactionRequest> handler) : Endpoint<UpdateTransactionRequest>
{
    public override void Configure()
    {
        Patch("/transaction/update");
    }

    public override async Task HandleAsync(UpdateTransactionRequest req, CancellationToken ct)
    {
        Result result = await handler.Handle(req, ct);

        await this.SendResponse(result);
    }
}
