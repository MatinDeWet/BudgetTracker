using Application.Features.AccountFeatures.DeleteAccount;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.AccountEndpoints.DeleteAccount;

public class DeleteAccountRequestValidator : Validator<DeleteAccountRequest>
{
    public DeleteAccountRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
