using API.Common.Validation;
using Application.Features.AccountFeatures.CreateAccount;
using FastEndpoints;

namespace API.Endpoints.AccountEndpoints.CreateAccount;

public class CreateAccountRequestValidator : Validator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.Name)
            .StringInput(64);
    }
}
