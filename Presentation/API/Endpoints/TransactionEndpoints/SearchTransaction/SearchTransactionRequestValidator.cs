using API.Common.Validation;
using Application.Features.TransactionFeatures.SeachTransaction;
using FastEndpoints;

namespace API.Endpoints.TransactionEndpoints.SearchTransaction;

public class SearchTransactionRequestValidator : Validator<SeachTransactionRequest>
{
    public SearchTransactionRequestValidator()
    {
        this.ValidatePageableRequest();
    }
}
