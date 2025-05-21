using System.Security.Claims;

namespace Application.Common.IdentitySupport;
public interface IInfoSetter : IList<Claim>
{
    void SetUser(IEnumerable<Claim> claims);
}
