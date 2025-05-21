using Domain.Common.Enums;

namespace Application.Common.IdentitySupport;
public interface IIdentityInfo
{
    int GetIdentityId();

    bool HasRole(ApplicationRoleEnum role);

    bool HasValue(string name);

    string GetValue(string name);
}
