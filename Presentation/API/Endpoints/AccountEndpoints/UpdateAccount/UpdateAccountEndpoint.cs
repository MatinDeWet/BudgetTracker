using API.Common.Extensions;
using Application.Features.AccountFeatures.UpdateAccount;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.AccountEndpoints.UpdateAccount;

public class UpdateAccountEndpoint(Application.Common.Messaging.ICommandHandler<UpdateAccountRequest> handler) : Endpoint<UpdateAccountRequest>
{
    public override void Configure()
    {
        Patch("/account/update");
    }

    public override async Task HandleAsync(UpdateAccountRequest req, CancellationToken ct)
    {
        Result result = await handler.Handle(req, ct);

        await this.SendResponse(result);
    }
}
