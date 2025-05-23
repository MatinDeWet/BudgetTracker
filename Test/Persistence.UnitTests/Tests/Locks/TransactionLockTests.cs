using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Persistence.Common.Repository;
using Persistence.Data.Context;
using Persistence.Locks;
using Shouldly;

namespace Persistence.UnitTests.Tests.Locks;
public class TransactionLockTests
{
    private readonly Mock<BudgetContext> _mockContext;
    private readonly TransactionLock _lock;

    public TransactionLockTests()
    {
        _mockContext = new Mock<BudgetContext>();
        _lock = new TransactionLock(_mockContext.Object);
    }

    // Helper method to create test transactions with specific properties using reflection
    private Transaction CreateTestTransaction(Guid id, Guid accountId, string description = "Test Transaction", decimal amount = 100.00m, DateTime? date = null)
    {
        // Use the factory method to create a transaction
        var transaction = Transaction.Create(
            accountId,
            description,
            amount,
            date ?? DateTime.UtcNow
        );

        // Set the Id using reflection
        typeof(Transaction)
            .BaseType! // Aggregate<Guid> which inherits from Entity<Guid>
            .BaseType! // Entity<Guid>
            .GetProperty("Id")!
            .SetValue(transaction, id);

        return transaction;
    }

    // Helper method to create test accounts with specific properties
    private Account CreateTestAccount(Guid id, int userId, string name = "Test Account")
    {
        // Use the factory method to create an account with userId
        var account = Account.Create(userId, name);

        // Set the Id using reflection
        typeof(Account)
            .BaseType! // Entity<Guid>
            .GetProperty("Id")!
            .SetValue(account, id);

        return account;
    }

    [Fact]
    public async Task HasAccess_WhenAccountIdIsEmpty_ShouldReturnFalse()
    {
        // Arrange
        Transaction transaction = CreateTestTransaction(Guid.NewGuid(), Guid.Empty);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(transaction, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenAccountBelongsToAnotherUser_ShouldReturnFalse()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        Transaction transaction = CreateTestTransaction(Guid.NewGuid(), accountId);
        int identityId = 1;
        int differentUserId = 2;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Setup the account query result - no matching account for the user
        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, differentUserId)
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);

        // Act
        bool result = await _lock.HasAccess(transaction, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenAccountBelongsToUser_ShouldReturnTrue()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        Transaction transaction = CreateTestTransaction(Guid.NewGuid(), accountId);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Setup the account query result - matching account for the user
        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, identityId)
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);

        // Act
        bool result = await _lock.HasAccess(transaction, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Secured_ShouldReturnOnlyTransactionsForUserAccounts()
    {
        // Arrange
        int identityId = 1;
        int otherUserId = 2;

        // Create accounts for different users
        var userAccountId1 = Guid.NewGuid();
        var userAccountId2 = Guid.NewGuid();
        var otherUserAccountId = Guid.NewGuid();

        var accountList = new List<Account>
        {
            CreateTestAccount(userAccountId1, identityId, "Checking"),
            CreateTestAccount(userAccountId2, identityId, "Savings"),
            CreateTestAccount(otherUserAccountId, otherUserId, "Other User Account")
        };

        // Create transactions for different accounts
        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(Guid.NewGuid(), userAccountId1, "Salary", 1000.00m),
            CreateTestTransaction(Guid.NewGuid(), userAccountId1, "Rent", -500.00m),
            CreateTestTransaction(Guid.NewGuid(), userAccountId2, "Interest", 10.00m),
            CreateTestTransaction(Guid.NewGuid(), otherUserAccountId, "Should not see", 50.00m)
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);

        // Act
        var result = _lock.Secured(identityId).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(3);
        result.All(t => t.AccountId == userAccountId1 || t.AccountId == userAccountId2).ShouldBeTrue();
        result.Any(t => t.AccountId == otherUserAccountId).ShouldBeFalse();
    }

    [Fact]
    public void Secured_WhenNoMatchingAccounts_ShouldReturnEmptyResult()
    {
        // Arrange
        int identityId = 999; // Non-existent user ID
        int otherUserId = 1;

        var otherUserAccountId = Guid.NewGuid();

        var accountList = new List<Account>
        {
            CreateTestAccount(otherUserAccountId, otherUserId, "Other User Account")
        };

        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(Guid.NewGuid(), otherUserAccountId, "Should not see", 50.00m)
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);

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

        var accountId = Guid.NewGuid();

        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, identityId, "Checking")
        };

        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(Guid.NewGuid(), accountId, "Grocery", -50.00m),
            CreateTestTransaction(Guid.NewGuid(), accountId, "Salary", 1000.00m),
            CreateTestTransaction(Guid.NewGuid(), accountId, "Dining", -75.00m)
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);

        // Act
        var result = _lock.Secured(identityId)
            .Where(t => t.Amount < 0) // Only expenses
            .ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.All(t => t.Amount < 0).ShouldBeTrue();
    }

    [Fact]
    public void Secured_ShouldAllowOrderingAndLimiting()
    {
        // Arrange
        int identityId = 1;

        var accountId = Guid.NewGuid();

        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, identityId, "Checking")
        };

        DateTime today = DateTime.UtcNow;
        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(Guid.NewGuid(), accountId, "Oldest", 100.00m, today.AddDays(-5)),
            CreateTestTransaction(Guid.NewGuid(), accountId, "Middle", 200.00m, today.AddDays(-3)),
            CreateTestTransaction(Guid.NewGuid(), accountId, "Recent", 300.00m, today.AddDays(-1))
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);

        // Act
        var result = _lock.Secured(identityId)
            .OrderByDescending(t => t.Date) // Most recent first
            .Take(2)
            .ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result[0].Description.ShouldBe("Recent");
        result[1].Description.ShouldBe("Middle");
    }
}
