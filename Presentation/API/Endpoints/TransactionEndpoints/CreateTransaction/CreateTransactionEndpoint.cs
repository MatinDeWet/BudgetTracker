using API.Common.Extensions;
using Application.Features.TransactionFeatures.CreateTransaction;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.TransactionEndpoints.CreateTransaction;

public class CreateTransactionEndpoint(Application.Common.Messaging.ICommandHandler<CreateTransactionRequest, CreateTransactionResponse> handler) : Endpoint<CreateTransactionRequest, CreateTransactionResponse>
{
    public override void Configure()
    {
        Post("/transaction/create");
    }

    public override async Task HandleAsync(CreateTransactionRequest req, CancellationToken ct)
    {
        Result<CreateTransactionResponse> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
