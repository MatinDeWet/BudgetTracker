using Application.Features.TransactionFeatures.GetTransactionById;
using FastEndpoints;
using FluentValidation;

namespace API.Endpoints.TransactionEndpoints.GetTransactionById;

public class GetTransactionByIdRequestValidator : Validator<GetTransactionByIdRequest>
{
    public GetTransactionByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
