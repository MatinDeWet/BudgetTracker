using Application.Common.Pagination;
using Domain.Common.Messaging;

namespace Application.Features.TransactionFeatures.SeachTransaction;
public sealed class SeachTransactionRequest : PageableRequest, IQuery<PageableResponse<SeachTransactionResponse>>
{
}
