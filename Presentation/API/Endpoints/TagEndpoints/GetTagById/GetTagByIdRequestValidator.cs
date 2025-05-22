using Application.Features.TagFeaturtes.GetTagById;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TagEndpoints.GetTagById;

public class GetTagByIdRequestValidator : Validator<GetTagByIdRequest>
{
    public GetTagByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
