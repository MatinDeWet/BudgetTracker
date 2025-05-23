using Application.Features.TransactionFeatures.UntagTransaction;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TransactionEndpoints.UntagTransaction;

public class UntagTransactionRequestValidator : Validator<UntagTransactionRequest>
{
    public UntagTransactionRequestValidator()
    {
        RuleFor(x => x.TagId)
            .NotEmpty();

        RuleFor(x => x.TransactionId)
            .NotEmpty();
    }
}
