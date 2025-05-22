using API.Common.Validation;
using Application.Features.AccountFeatures.UpdateAccount;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.AccountEndpoints.UpdateAccount;

public class UpdateAccountRequestValidator : Validator<UpdateAccountRequest>
{
    public UpdateAccountRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .StringInput(64);
    }
}
