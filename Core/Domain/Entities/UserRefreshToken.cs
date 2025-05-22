using Domain.Common.Abstractions;

namespace Domain.Entities;
public class UserRefreshToken : Entity<Guid>
{
    public int UserID { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }

    public static UserRefreshToken Create(int userId, string token, DateTime expirationDate)
    {
        return new UserRefreshToken
        {
            Id = Guid.CreateVersion7(),
            UserID = userId,
            Token = token,
            ExpiryDate = expirationDate,
        };
    }
}
