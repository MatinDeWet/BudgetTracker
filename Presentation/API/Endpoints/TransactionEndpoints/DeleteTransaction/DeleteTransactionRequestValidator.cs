using Application.Features.TransactionFeatures.DeleteTransaction;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TransactionEndpoints.DeleteTransaction;

public class DeleteTransactionRequestValidator : Validator<DeleteTransactionRequest>
{
    public DeleteTransactionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
