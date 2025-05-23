using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Persistence.Common.Repository;
using Persistence.Data.Context;
using Persistence.Locks;
using Shouldly;

namespace Persistence.UnitTests.Tests.Locks;
public class TransactionTagLockTests
{
    private readonly Mock<BudgetContext> _mockContext;
    private readonly TransactionTagLock _lock;

    public TransactionTagLockTests()
    {
        _mockContext = new Mock<BudgetContext>();
        _lock = new TransactionTagLock(_mockContext.Object);
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

    // Helper method to create test tags with specific properties
    private Tag CreateTestTag(Guid id, int userId, string name = "Test Tag")
    {
        // Use the factory method to create a tag with userId and name
        var tag = Tag.Create(userId, name);

        // Set the Id using reflection
        typeof(Tag)
            .BaseType! // Entity<Guid>
            .GetProperty("Id")!
            .SetValue(tag, id);

        return tag;
    }

    // Helper method to create TransactionTag
    private TransactionTag CreateTestTransactionTag(Guid transactionId, Guid tagId)
    {
        return TransactionTag.Create(transactionId, tagId);
    }

    [Fact]
    public async Task HasAccess_WhenUserOwnsTransactionAndTag_ShouldReturnTrue()
    {
        // Arrange
        int identityId = 1;
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var tagId = Guid.NewGuid();

        TransactionTag transactionTag = CreateTestTransactionTag(transactionId, tagId);

        // Setup the transaction query
        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(transactionId, accountId)
        };

        // Setup the account query
        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, identityId)
        };

        // Setup the tag query
        var tagList = new List<Tag>
        {
            CreateTestTag(tagId, identityId)
        };

        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Tag>> mockTagDbSet = tagList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockTagDbSet.Object);

        // Act
        bool result = await _lock.HasAccess(transactionTag, identityId, RepositoryOperationEnum.Update, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task HasAccess_WhenUserDoesNotOwnTransaction_ShouldReturnFalse()
    {
        // Arrange
        int identityId = 1;
        int otherUserId = 2;
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var tagId = Guid.NewGuid();

        TransactionTag transactionTag = CreateTestTransactionTag(transactionId, tagId);

        // Setup the transaction query
        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(transactionId, accountId)
        };

        // Setup the account query - different user owns the account
        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, otherUserId)
        };

        // Setup the tag query - current user owns the tag
        var tagList = new List<Tag>
        {
            CreateTestTag(tagId, identityId)
        };

        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Tag>> mockTagDbSet = tagList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockTagDbSet.Object);

        // Act
        bool result = await _lock.HasAccess(transactionTag, identityId, RepositoryOperationEnum.Update, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenUserDoesNotOwnTag_ShouldReturnFalse()
    {
        // Arrange
        int identityId = 1;
        int otherUserId = 2;
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var tagId = Guid.NewGuid();

        TransactionTag transactionTag = CreateTestTransactionTag(transactionId, tagId);

        // Setup the transaction query
        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(transactionId, accountId)
        };

        // Setup the account query - current user owns the account
        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, identityId)
        };

        // Setup the tag query - different user owns the tag
        var tagList = new List<Tag>
        {
            CreateTestTag(tagId, otherUserId)
        };

        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Tag>> mockTagDbSet = tagList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockTagDbSet.Object);

        // Act
        bool result = await _lock.HasAccess(transactionTag, identityId, RepositoryOperationEnum.Update, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenTransactionDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        int identityId = 1;
        var nonExistentTransactionId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        var accountId = Guid.NewGuid(); // Add account ID even though transaction doesn't exist

        TransactionTag transactionTag = CreateTestTransactionTag(nonExistentTransactionId, tagId);

        // Setup the transaction query - empty result (transaction doesn't exist)
        var transactionList = new List<Transaction>();

        // Setup the account query - even though no transaction references it
        var accountList = new List<Account>
    {
        CreateTestAccount(accountId, identityId)
    };

        // Setup the tag query
        var tagList = new List<Tag>
    {
        CreateTestTag(tagId, identityId)
    };

        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Tag>> mockTagDbSet = tagList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockTagDbSet.Object);

        // Act
        bool result = await _lock.HasAccess(transactionTag, identityId, RepositoryOperationEnum.Update, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenTagDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        int identityId = 1;
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var nonExistentTagId = Guid.NewGuid();

        TransactionTag transactionTag = CreateTestTransactionTag(transactionId, nonExistentTagId);

        // Setup the transaction query
        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(transactionId, accountId)
        };

        // Setup the account query
        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, identityId)
        };

        // Setup the tag query - empty result
        var tagList = new List<Tag>();

        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Tag>> mockTagDbSet = tagList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);
        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockTagDbSet.Object);

        // Act
        bool result = await _lock.HasAccess(transactionTag, identityId, RepositoryOperationEnum.Update, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Secured_ShouldReturnOnlyTransactionTagsWhereUserOwnsTransactionAndTag()
    {
        // Arrange
        int identityId = 1;
        int otherUserId = 2;

        // Create accounts for different users
        var userAccountId = Guid.NewGuid();
        var otherUserAccountId = Guid.NewGuid();

        var accountList = new List<Account>
        {
            CreateTestAccount(userAccountId, identityId),
            CreateTestAccount(otherUserAccountId, otherUserId)
        };

        // Create transactions for different accounts
        var userTransactionId1 = Guid.NewGuid();
        var userTransactionId2 = Guid.NewGuid();
        var otherUserTransactionId = Guid.NewGuid();

        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(userTransactionId1, userAccountId),
            CreateTestTransaction(userTransactionId2, userAccountId),
            CreateTestTransaction(otherUserTransactionId, otherUserAccountId)
        };

        // Create tags for different users
        var userTagId1 = Guid.NewGuid();
        var userTagId2 = Guid.NewGuid();
        var otherUserTagId = Guid.NewGuid();

        var tagList = new List<Tag>
        {
            CreateTestTag(userTagId1, identityId, "Food"),
            CreateTestTag(userTagId2, identityId, "Travel"),
            CreateTestTag(otherUserTagId, otherUserId, "Shopping")
        };

        // Create transaction tags with various combinations
        var transactionTagList = new List<TransactionTag>
        {
            // Valid combinations - user owns both transaction and tag
            CreateTestTransactionTag(userTransactionId1, userTagId1),  // Should be returned
            CreateTestTransactionTag(userTransactionId2, userTagId2),  // Should be returned
            
            // Invalid combinations - user doesn't own transaction
            CreateTestTransactionTag(otherUserTransactionId, userTagId1),
            
            // Invalid combinations - user doesn't own tag
            CreateTestTransactionTag(userTransactionId1, otherUserTagId),
            
            // Invalid combinations - user owns neither
            CreateTestTransactionTag(otherUserTransactionId, otherUserTagId)
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Tag>> mockTagDbSet = tagList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<TransactionTag>> mockTransactionTagDbSet = transactionTagList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockTagDbSet.Object);
        _mockContext.Setup(x => x.Set<TransactionTag>()).Returns(mockTransactionTagDbSet.Object);

        // Act
        var result = _lock.Secured(identityId).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);

        // Check that only valid combinations are returned
        result.ShouldContain(tt => tt.TransactionId == userTransactionId1 && tt.TagId == userTagId1);
        result.ShouldContain(tt => tt.TransactionId == userTransactionId2 && tt.TagId == userTagId2);
    }

    [Fact]
    public void Secured_WhenNoMatchingTransactionTags_ShouldReturnEmptyResult()
    {
        // Arrange
        int identityId = 999; // Non-existent user ID
        int existingUserId = 1;

        // Create account, transaction, and tag for an existing user
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var tagId = Guid.NewGuid();

        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, existingUserId)
        };

        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(transactionId, accountId)
        };

        var tagList = new List<Tag>
        {
            CreateTestTag(tagId, existingUserId)
        };

        var transactionTagList = new List<TransactionTag>
        {
            CreateTestTransactionTag(transactionId, tagId)
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Tag>> mockTagDbSet = tagList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<TransactionTag>> mockTransactionTagDbSet = transactionTagList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockTagDbSet.Object);
        _mockContext.Setup(x => x.Set<TransactionTag>()).Returns(mockTransactionTagDbSet.Object);

        // Act
        var result = _lock.Secured(identityId).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void Secured_WithComplexQueries_ShouldPreserveTheQueryability()
    {
        // Arrange
        int identityId = 1;

        // Create accounts
        var accountId = Guid.NewGuid();
        var accountList = new List<Account>
        {
            CreateTestAccount(accountId, identityId)
        };

        // Create transactions
        var transactionId1 = Guid.NewGuid();
        var transactionId2 = Guid.NewGuid();
        var transactionList = new List<Transaction>
        {
            CreateTestTransaction(transactionId1, accountId, "Groceries", -50.00m),
            CreateTestTransaction(transactionId2, accountId, "Dining", -75.00m)
        };

        // Create tags
        var foodTagId = Guid.NewGuid();
        var essentialTagId = Guid.NewGuid();
        var luxuryTagId = Guid.NewGuid();
        var tagList = new List<Tag>
        {
            CreateTestTag(foodTagId, identityId, "Food"),
            CreateTestTag(essentialTagId, identityId, "Essential"),
            CreateTestTag(luxuryTagId, identityId, "Luxury")
        };

        // Create transaction tags
        var transactionTagList = new List<TransactionTag>
        {
            // Groceries is tagged as Food and Essential
            CreateTestTransactionTag(transactionId1, foodTagId),
            CreateTestTransactionTag(transactionId1, essentialTagId),
            
            // Dining is tagged as Food and Luxury
            CreateTestTransactionTag(transactionId2, foodTagId),
            CreateTestTransactionTag(transactionId2, luxuryTagId)
        };

        Mock<DbSet<Account>> mockAccountDbSet = accountList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Transaction>> mockTransactionDbSet = transactionList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<Tag>> mockTagDbSet = tagList.AsQueryable().BuildMockDbSet();
        Mock<DbSet<TransactionTag>> mockTransactionTagDbSet = transactionTagList.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<Account>()).Returns(mockAccountDbSet.Object);
        _mockContext.Setup(x => x.Set<Transaction>()).Returns(mockTransactionDbSet.Object);
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockTagDbSet.Object);
        _mockContext.Setup(x => x.Set<TransactionTag>()).Returns(mockTransactionTagDbSet.Object);

        // Act - Get transaction tags for Food but only Luxury items
        var result = _lock.Secured(identityId)
            .Where(tt => tt.TagId == foodTagId)
            .Where(tt => tt.TransactionId == transactionId2)
            .ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].TransactionId.ShouldBe(transactionId2);
        result[0].TagId.ShouldBe(foodTagId);
    }
}
