using API.Common.Validation;
using Application.Features.TagFeaturtes.SearchTag;
using FastEndpoints;

namespace API.Endpoints.TagEndpoints.SearchTag;

public class SearchTagRequestValidator : Validator<SearchTagRequest>
{
    public SearchTagRequestValidator()
    {
        this.ValidatePageableRequest();
    }
}
