using Domain.Common.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public partial class ApplicationRoleConfiguration
{
    partial void OnConfigurePartial(EntityTypeBuilder<ApplicationRole> entity)
    {
        var roles = new List<ApplicationRole>();

        IEnumerable<ApplicationRoleEnum> roleEnums = Enum.GetValues<ApplicationRoleEnum>()
            .Where(r => r != ApplicationRoleEnum.None);

        foreach (ApplicationRoleEnum roleEnum in roleEnums)
        {
            // Convert enum name to string for the role name
            string roleName = roleEnum.ToString();

            roles.Add(new ApplicationRole
            {
                Id = (int)roleEnum,
                Name = roleName,
                NormalizedName = roleName.ToUpper(System.Globalization.CultureInfo.CurrentCulture)
            });
        }

        entity.HasData(roles);
    }
}
