using Application.Features.TransactionFeatures.TagTransaction;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TransactionEndpoints.TagTransaction;

public class TagTransactionRequestValidator : Validator<TagTransactionRequest>
{
    public TagTransactionRequestValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty();

        RuleFor(x => x.TagId)
            .NotEmpty();
    }
}
