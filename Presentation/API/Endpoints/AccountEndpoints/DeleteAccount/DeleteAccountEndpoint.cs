using API.Common.Extensions;
using Application.Features.AccountFeatures.DeleteAccount;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.AccountEndpoints.DeleteAccount;

public class DeleteAccountEndpoint(Application.Common.Messaging.ICommandHandler<DeleteAccountRequest> handler) : Endpoint<DeleteAccountRequest>
{
    public override void Configure()
    {
        Delete("account/{Id}");
    }

    public override async Task HandleAsync(DeleteAccountRequest req, CancellationToken ct)
    {
        Result result = await handler.Handle(req, ct);

        await this.SendResponse(result);
    }
}
