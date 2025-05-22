using Application.Common.Models;
using Application.Common.Pagination;
using Domain.Common.Messaging;
using Domain.Enums;

namespace Application.Features.TransactionFeatures.SeachTransaction;
public sealed class SeachTransactionRequest : PageableRequest, IQuery<PageableResponse<SeachTransactionResponse>>
{
    public Guid? AccountId { get; set; }

    public TransactionDirectionEnum? TransactionDirection { get; set; }

    public DateRange? DateRange { get; set; }
}
