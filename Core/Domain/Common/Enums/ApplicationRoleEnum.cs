namespace Domain.Common.Enums;
public enum ApplicationRoleEnum
{
    None = 0,
    Admin = 1,
    SuperAdmin = Admin | 2
}
