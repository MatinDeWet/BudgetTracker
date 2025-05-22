using API.Common.Validation;
using Application.Features.TransactionFeatures.CreateTransaction;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TransactionEndpoints.CreateTransaction;

public class CreateTransactionRequestValidator : Validator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty();

        RuleFor(x => x.Description)
            .StringInput(512);

        RuleFor(x => x.Amount)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEmpty();
    }
}
