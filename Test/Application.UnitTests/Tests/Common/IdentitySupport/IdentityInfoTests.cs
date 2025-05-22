using System.Security.Claims;
using Application.Common.IdentitySupport;
using Domain.Common.Enums;

namespace Application.UnitTests.Tests.Common.IdentitySupport;
public class IdentityInfoTests
{
    [Fact]
    public void GetIdentityId_WithValidId_ReturnsId()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser(
        [
            new (ClaimTypes.NameIdentifier, "123")
        ]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        int result = identityInfo.GetIdentityId();

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public void GetIdentityId_WithInvalidId_ReturnsZero()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser(
        [
            new (ClaimTypes.NameIdentifier, "invalid")
        ]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        int result = identityInfo.GetIdentityId();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetIdentityId_WithNoId_ReturnsZero()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser([]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        int result = identityInfo.GetIdentityId();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void HasRole_WithMatchingRole_ReturnsTrue()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser(
        [
            new (ClaimTypes.Role, "Admin")
        ]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        bool result = identityInfo.HasRole(ApplicationRoleEnum.Admin);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasRole_WithSuperAdminRole_ReturnsTrueForAdminCheck()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser(
        [
            new Claim(ClaimTypes.Role, ApplicationRoleEnum.SuperAdmin.ToString())
        ]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        bool resultForAdmin = identityInfo.HasRole(ApplicationRoleEnum.Admin);
        bool resultForSuperAdmin = identityInfo.HasRole(ApplicationRoleEnum.SuperAdmin);

        // Assert
        Assert.True(resultForAdmin);
        Assert.True(resultForSuperAdmin);
    }

    [Fact]
    public void HasRole_WithNonMatchingRole_ReturnsFalse()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser(
            [
                new Claim(ClaimTypes.Role, "None")
            ]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        bool result = identityInfo.HasRole(ApplicationRoleEnum.Admin);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetValue_WithExistingClaim_ReturnsValue()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser(
        [
            new Claim(ClaimTypes.Name, "TestUser")
        ]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        string result = identityInfo.GetValue(ClaimTypes.Name);

        // Assert
        Assert.Equal("TestUser", result);
    }

    [Fact]
    public void GetValue_WithNonExistingClaim_ReturnsNull()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser([]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        string result = identityInfo.GetValue(ClaimTypes.Name);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void HasValue_WithExistingClaim_ReturnsTrue()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser(
        [
            new Claim(ClaimTypes.Name, "TestUser")
        ]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        bool result = identityInfo.HasValue(ClaimTypes.Name);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasValue_WithNonExistingClaim_ReturnsFalse()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        infoSetter.SetUser([]);
        var identityInfo = new IdentityInfo(infoSetter);

        // Act
        bool result = identityInfo.HasValue(ClaimTypes.Name);

        // Assert
        Assert.False(result);
    }
}
