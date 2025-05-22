using Application.Features.AccountFeatures.GetAccountById;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.AccountEndpoints.GetAccountById;

public class GetAccountByIdRequestValidator : Validator<GetAccountByIdRequest>
{
    public GetAccountByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
