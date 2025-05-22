using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Ardalis.Result;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TagFeaturtes.DeleteTag;
internal sealed class DeleteTagHandler(ITagQueryRepository queryRepo, ITagCommandRepository commandRepo) : ICommandHandler<DeleteTagRequest>
{
    public async Task<Result> Handle(DeleteTagRequest command, CancellationToken cancellationToken)
    {
        Tag? tag = await queryRepo.Tags
            .Where(x => x.Id == command.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (tag is null)
        {
            return Result.NotFound();
        }

        await commandRepo.DeleteAsync(tag, true, cancellationToken);

        return Result.Success();
    }
}
