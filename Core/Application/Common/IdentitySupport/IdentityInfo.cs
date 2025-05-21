using System.Security.Claims;
using Domain.Common.Enums;

namespace Application.Common.IdentitySupport;
public class IdentityInfo : IIdentityInfo
{
    private readonly IInfoSetter _infoSetter;

    public IdentityInfo(IInfoSetter infoSetter)
    {
        _infoSetter = infoSetter;
    }

    public int GetIdentityId()
    {
        string uid = GetValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(uid, out int result))
        {
            return 0;
        }

        return result;
    }

    public bool HasRole(ApplicationRoleEnum role)
    {
        var roles = _infoSetter
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList();

        ApplicationRoleEnum combinedRoles = ApplicationRoleEnum.None;

        foreach (string? roleString in roles)
        {
            if (Enum.TryParse(roleString, true, out ApplicationRoleEnum parsedRole))
            {
                combinedRoles |= parsedRole;
            }
        }

        return combinedRoles.HasFlag(role);
    }

    public string GetValue(string name)
    {
        Claim? claim = _infoSetter.FirstOrDefault(x => x.Type == name);
        return claim == null ? null! : claim.Value;
    }

    public bool HasValue(string name)
    {
        return _infoSetter.Any(x => x.Type == name);
    }
}
