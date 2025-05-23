namespace Application.Features.UserFeatures.GetUserById;
public sealed record GetUserByIdResponse
{
    public int Id { get; set; }

    public string Email { get; set; }
}
