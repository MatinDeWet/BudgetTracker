using API.Common.Validation;
using Application.Features.TagFeaturtes.UpdateTag;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TagEndpoints.UpdateTag;

public class UpdateTagRequestValidator : Validator<UpdateTagRequest>
{
    public UpdateTagRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .StringInput(64);
    }
}
