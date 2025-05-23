using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Persistence.Common.Repository;
using Persistence.Data.Context;
using Persistence.Locks;
using Shouldly;

namespace Persistence.UnitTests.Tests.Locks;
public class AccountLockTests
{
    private readonly Mock<BudgetContext> _mockContext;
    private readonly AccountLock _lock;

    public AccountLockTests()
    {
        _mockContext = new Mock<BudgetContext>();
        _lock = new AccountLock(_mockContext.Object);
    }

    // Helper method to create test accounts with specific properties using reflection
    private Account CreateTestAccount(Guid id, int userId)
    {
        // Use the factory method to create an account with userId
        var account = Account.Create(userId, "Test Account");

        // Set the Id using reflection
        typeof(Account)
            .BaseType! // Entity<Guid>
            .GetProperty("Id")!
            .SetValue(account, id);

        return account;
    }

    [Fact]
    public async Task HasAccess_WhenUserIdIsZero_ShouldReturnFalse()
    {
        // Arrange
        Account account = CreateTestAccount(Guid.NewGuid(), 0);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(account, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenUserIdDoesNotMatchIdentityId_ShouldReturnFalse()
    {
        // Arrange
        Account account = CreateTestAccount(Guid.NewGuid(), 2);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(account, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenUserIdMatchesIdentityId_ShouldReturnTrue()
    {
        // Arrange
        Account account = CreateTestAccount(Guid.NewGuid(), 1);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(account, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Secured_ShouldReturnOnlyAccountsWithMatchingUserId()
    {
        // Arrange
        int identityId = 1;
        var accountList = new List<Account>
        {
            CreateTestAccount(Guid.NewGuid(), identityId),
            CreateTestAccount(Guid.NewGuid(), identityId),
            CreateTestAccount(Guid.NewGuid(), 2),
            CreateTestAccount(Guid.NewGuid(), 3)
        };

        Mock<DbSet<Account>> mockDbSet = accountList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockDbSet.Object);

        // Act
        var result = _lock.Secured(identityId).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.All(a => a.UserId == identityId).ShouldBeTrue();
    }

    [Fact]
    public void Secured_WhenNoAccountsWithMatchingUserId_ShouldReturnEmptyResult()
    {
        // Arrange
        int identityId = 999; // Non-existent user ID
        var accountList = new List<Account>
        {
            CreateTestAccount(Guid.NewGuid(), 1),
            CreateTestAccount(Guid.NewGuid(), 2),
            CreateTestAccount(Guid.NewGuid(), 3)
        };

        Mock<DbSet<Account>> mockDbSet = accountList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockDbSet.Object);

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
        var accountList = new List<Account>
        {
            CreateTestAccount(Guid.NewGuid(), identityId),
        };

        // Update the name of the first account
        accountList[0].Update("Savings Account");

        // Add more accounts
        Account checkingAccount = CreateTestAccount(Guid.NewGuid(), identityId);
        checkingAccount.Update("Checking Account");
        accountList.Add(checkingAccount);

        Account otherUserAccount = CreateTestAccount(Guid.NewGuid(), 2);
        otherUserAccount.Update("Savings Account");
        accountList.Add(otherUserAccount);

        Mock<DbSet<Account>> mockDbSet = accountList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockDbSet.Object);

        // Act
        var result = _lock.Secured(identityId)
            .Where(a => a.Name.Contains("Savings"))
            .ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].UserId.ShouldBe(identityId);
        result[0].Name.ShouldBe("Savings Account");
    }
}
