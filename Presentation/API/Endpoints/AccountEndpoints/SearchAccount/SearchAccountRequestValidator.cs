using API.Common.Validation;
using Application.Features.AccountFeatures.SearchAccount;
using FastEndpoints;

namespace API.Endpoints.AccountEndpoints.SearchAccount;

public class SearchAccountRequestValidator : Validator<SearchAccountRequest>
{
    public SearchAccountRequestValidator()
    {
        this.ValidatePageableRequest();
    }
}
