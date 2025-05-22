namespace Application.Features.TagFeaturtes.GetTagById;
public sealed record GetTagByIdResponse
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public DateTimeOffset DateCreated { get; set; }
}
