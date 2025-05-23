using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Persistence.Common.Repository;
using Persistence.Data.Context;
using Persistence.Locks;
using Shouldly;

namespace Persistence.UnitTests.Tests.Locks;
public class UserLockTests
{
    private readonly Mock<BudgetContext> _mockContext;
    private readonly UserLock _lock;

    public UserLockTests()
    {
        _mockContext = new Mock<BudgetContext>();
        _lock = new UserLock(_mockContext.Object);
    }

    // Helper method to create test users with specific IDs using reflection
    private User CreateUserWithId(int id)
    {
        var user = new User();
        typeof(User)
            .BaseType! // Entity<int>
            .GetProperty("Id")!
            .SetValue(user, id);

        return user;
    }

    [Fact]
    public async Task HasAccess_WhenInsert_ShouldReturnTrue()
    {
        // Arrange
        var user = new User(); // For insert, we don't need to set Id
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Insert;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(user, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task HasAccess_WhenUserIdIsZero_ShouldReturnFalse()
    {
        // Arrange
        User user = CreateUserWithId(0);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(user, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenUserIdIsNotIdentityId_ShouldReturnFalse()
    {
        // Arrange
        User user = CreateUserWithId(2);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(user, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenUserIdIsIdentityId_ShouldReturnTrue()
    {
        // Arrange
        User user = CreateUserWithId(1);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(user, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Secured_ShouldReturnOnlyTheUserWithMatchingIdentityId()
    {
        // Arrange
        int identityId = 1;

        var userList = new List<User>
        {
            CreateUserWithId(identityId),
            CreateUserWithId(2),
            CreateUserWithId(3)
        };

        Mock<DbSet<User>> mockDbSet = userList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<User>()).Returns(mockDbSet.Object);

        // Act
        var result = _lock.Secured(identityId).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(identityId);
    }

    [Fact]
    public void Secured_WhenNoUserWithMatchingIdentityId_ShouldReturnEmptyResult()
    {
        // Arrange
        int identityId = 999; // Non-existent user ID

        var userList = new List<User>
        {
            CreateUserWithId(1),
            CreateUserWithId(2),
            CreateUserWithId(3)
        };

        Mock<DbSet<User>> mockDbSet = userList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<User>()).Returns(mockDbSet.Object);

        // Act
        var result = _lock.Secured(identityId).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void Secured_ShouldRespectQueryFiltering()
    {
        // Arrange
        int identityId = 1;

        // Create users with IDs and ApplicationUser data
        User user1 = CreateUserWithId(identityId);
        user1.IdentityInfo = new ApplicationUser { Email = "user1@example.com" };

        User user2 = CreateUserWithId(2);
        user2.IdentityInfo = new ApplicationUser { Email = "user2@example.com" };

        User user3 = CreateUserWithId(3);
        user3.IdentityInfo = new ApplicationUser { Email = "user3@example.com" };

        var userList = new List<User> { user1, user2, user3 };

        Mock<DbSet<User>> mockDbSet = userList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<User>()).Returns(mockDbSet.Object);

        // Act
        var result = _lock.Secured(identityId)
            .Where(u => u.IdentityInfo.Email!.Contains("user1"))
            .ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(identityId);
        result[0].IdentityInfo.Email.ShouldBe("user1@example.com");
    }
}
