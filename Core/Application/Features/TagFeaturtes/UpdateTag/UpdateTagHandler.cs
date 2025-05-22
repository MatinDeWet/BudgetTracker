using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Ardalis.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TagFeaturtes.UpdateTag;
internal sealed class UpdateTagHandler(ITagQueryRepository queryRepo, ITagCommandRepository commandRepo) : ICommandHandler<UpdateTagRequest>
{
    public async Task<Result> Handle(UpdateTagRequest command, CancellationToken cancellationToken)
    {
        Tag? tag = await queryRepo.Tags
            .Where(x => x.Id == command.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (tag is null)
        {
            return Result.NotFound();
        }

        tag.Update(command.Name);

        await commandRepo.UpdateAsync(tag, true, cancellationToken);

        return Result.Success();
    }
}
