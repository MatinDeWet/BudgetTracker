using Application.Common.Pagination;
using Domain.Common.Messaging;

namespace Application.Features.AccountFeatures.SearchAccount;
public sealed class SearchAccountRequest : PageableRequest, IQuery<PageableResponse<SearchAccountResponse>>
{
}
