using Domain.Common.Messaging;

namespace Application.Features.TagFeaturtes.CreateTag;
public sealed record CreateTagRequest(string Name) : ICommand<CreateTagResponse>;
