using Domain.Common.Messaging;

namespace Application.Features.TagFeaturtes.GetTagById;
public sealed record GetTagByIdRequest(Guid Id) : IQuery<GetTagByIdResponse>;
