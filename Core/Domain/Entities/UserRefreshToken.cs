namespace Domain.Entities;
public class UserRefreshToken
{
    public int UserID { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
}
