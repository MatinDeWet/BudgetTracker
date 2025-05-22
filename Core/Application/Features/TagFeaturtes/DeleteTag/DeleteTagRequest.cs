using Domain.Common.Messaging;

namespace Application.Features.TagFeaturtes.DeleteTag;
public sealed record DeleteTagRequest(Guid Id) : ICommand;
