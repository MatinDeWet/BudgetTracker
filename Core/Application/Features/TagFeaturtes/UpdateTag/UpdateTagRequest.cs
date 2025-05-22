using Domain.Common.Messaging;

namespace Application.Features.TagFeaturtes.UpdateTag;
public sealed record UpdateTagRequest(Guid Id, string Name) : ICommand;
