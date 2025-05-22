using API.Common.Validation;
using Application.Features.TagFeaturtes.CreateTag;
using FastEndpoints;

namespace API.Endpoints.TagEndpoints.CreateTag;

public class CreateTagRequestValidator : Validator<CreateTagRequest>
{
    public CreateTagRequestValidator()
    {
        RuleFor(x => x.Name)
            .StringInput(64);
    }
}
