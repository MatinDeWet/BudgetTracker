using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common.IdentitySupport;

namespace Application.UnitTests.Tests.Common.IdentitySupport;
public class InfoSetterTests
{
    [Fact]
    public void SetUser_ShouldClearExistingAndAddNewClaims()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        var initialClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "InitialUser")
        };
        infoSetter.AddRange(initialClaims);

        var newClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "NewUser"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        // Act
        infoSetter.SetUser(newClaims);

        // Assert
        Assert.Equal(2, infoSetter.Count);
        Assert.Equal("NewUser", infoSetter[0].Value);
        Assert.Equal("Admin", infoSetter[1].Value);
    }

    [Fact]
    public void Clear_ShouldRemoveAllClaims()
    {
        // Arrange
        var infoSetter = new InfoSetter
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        // Act
        infoSetter.Clear();

        // Assert
        Assert.Empty(infoSetter);
    }

    [Fact]
    public void AddRange_ShouldAddAllClaimsToCollection()
    {
        // Arrange
        var infoSetter = new InfoSetter();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.NameIdentifier, "123")
        };

        // Act
        infoSetter.AddRange(claims);

        // Assert
        Assert.Equal(3, infoSetter.Count);
        Assert.Contains(infoSetter, c => c.Type == ClaimTypes.Name && c.Value == "TestUser");
        Assert.Contains(infoSetter, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        Assert.Contains(infoSetter, c => c.Type == ClaimTypes.NameIdentifier && c.Value == "123");
    }
}
