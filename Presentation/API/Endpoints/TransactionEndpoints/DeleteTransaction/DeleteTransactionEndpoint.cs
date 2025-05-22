using API.Common.Extensions;
using Application.Features.TransactionFeatures.DeleteTransaction;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TransactionEndpoints.DeleteTransaction;

public class DeleteTransactionEndpoint(Application.Common.Messaging.ICommandHandler<DeleteTransactionRequest> handler) : Endpoint<DeleteTransactionRequest>
{
    public override void Configure()
    {
        Delete("transaction/{Id}");
    }

    public override async Task HandleAsync(DeleteTransactionRequest req, CancellationToken ct)
    {
        Result result = await handler.Handle(req, ct);

        await this.SendResponse(result);
    }
}
