using Domain.Common.Abstractions;

namespace Domain.Entities;
public class UserRefreshToken : Entity<Guid>
{
    public int UserID { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}
