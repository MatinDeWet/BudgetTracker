using API.Common.Validation;
using Application.Features.TransactionFeatures.UpdateTransaction;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TransactionEndpoints.UpdateTransaction;

public class UpdateTransactionValidator : Validator<UpdateTransactionRequest>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Description)
            .StringInput(512);

        RuleFor(x => x.Amount)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEmpty();
    }
}

