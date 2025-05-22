using Application.Common.Pagination;
using Domain.Common.Messaging;

namespace Application.Features.TagFeaturtes.SearchTag;
public sealed class SearchTagRequest : PageableRequest, IQuery<PageableResponse<SearchTagResponse>>
{
}
