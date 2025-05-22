namespace Application.Features.AccountFeatures.GetAccountById;
public sealed record GetAccountByIdResponse
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public DateTimeOffset DateCreated { get; set; }
}
