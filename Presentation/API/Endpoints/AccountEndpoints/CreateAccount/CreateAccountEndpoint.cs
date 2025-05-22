using API.Common.Extensions;
using Application.Features.AccountFeatures.CreateAccount;
using Ardalis.Result;
using FastEndpoints;

namespace API.Endpoints.AccountEndpoints.CreateAccount;

public class CreateAccountEndpoint(Application.Common.Messaging.ICommandHandler<CreateAccountRequest, CreateAccountResponse> handler) : Endpoint<CreateAccountRequest, CreateAccountResponse>
{
    public override void Configure()
    {
        Post("/account/create");
    }

    public override async Task HandleAsync(CreateAccountRequest req, CancellationToken ct)
    {
        Result<CreateAccountResponse> result = await handler.Handle(req, ct);

        await this.SendResponse(result, response => response.GetValue());
    }
}
