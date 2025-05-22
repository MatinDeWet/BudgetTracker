using Application.Features.TagFeaturtes.DeleteTag;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TagEndpoints.DeleteTag;

public class DeleteTagRequestValidator : Validator<DeleteTagRequest>
{
    public DeleteTagRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
