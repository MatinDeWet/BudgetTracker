using Application.Features.UserFeatures.GetUserById;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.UserEndpoints.GetUserById;

public class GetUserByIdRequestValidator : Validator<GetUserByIdRequest>
{
    public GetUserByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
