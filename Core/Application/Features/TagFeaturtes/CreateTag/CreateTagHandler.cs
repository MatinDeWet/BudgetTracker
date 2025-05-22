using Application.Common.IdentitySupport;
using Application.Common.Messaging;
using Application.Repositories.Command;
using Ardalis.Result;
using Domain.Entities;

namespace Application.Features.TagFeaturtes.CreateTag;
internal sealed class CreateTagHandler(ITagCommandRepository repo, IIdentityInfo info) : ICommandHandler<CreateTagRequest, CreateTagResponse>
{
    public async Task<Result<CreateTagResponse>> Handle(CreateTagRequest command, CancellationToken cancellationToken)
    {
        var newTag = Tag.Create(info.GetIdentityId(), command.Name);

        await repo.InsertAsync(newTag, true, cancellationToken);

        return Result.Created(new CreateTagResponse(newTag.Id));
    }
}
