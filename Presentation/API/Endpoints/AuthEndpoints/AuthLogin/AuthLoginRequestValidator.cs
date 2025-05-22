using API.Common.Validation;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.AuthEndpoints.AuthLogin;

public class AuthLoginRequestValidator : Validator<AuthLoginRequest>
{
    public AuthLoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .StringInput(256)
            .EmailAddress()
            .WithMessage("{PropertyName} is not valid");

        RuleFor(x => x.Password)
            .StringInput(512);
    }
}
