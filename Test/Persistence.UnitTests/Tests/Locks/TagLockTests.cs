using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Persistence.Common.Repository;
using Persistence.Data.Context;
using Persistence.Locks;
using Shouldly;

namespace Persistence.UnitTests.Tests.Locks;
public class TagLockTests
{
    private readonly Mock<BudgetContext> _mockContext;
    private readonly TagLock _lock;

    public TagLockTests()
    {
        _mockContext = new Mock<BudgetContext>();
        _lock = new TagLock(_mockContext.Object);
    }

    // Helper method to create test tags with specific properties using reflection
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

    [Fact]
    public async Task HasAccess_WhenUserIdIsZero_ShouldReturnFalse()
    {
        // Arrange
        Tag tag = CreateTestTag(Guid.NewGuid(), 0);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(tag, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenUserIdDoesNotMatchIdentityId_ShouldReturnFalse()
    {
        // Arrange
        Tag tag = CreateTestTag(Guid.NewGuid(), 2);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(tag, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasAccess_WhenUserIdMatchesIdentityId_ShouldReturnTrue()
    {
        // Arrange
        Tag tag = CreateTestTag(Guid.NewGuid(), 1);
        int identityId = 1;
        RepositoryOperationEnum operation = RepositoryOperationEnum.Update;
        CancellationToken cancellationToken = CancellationToken.None;

        // Act
        bool result = await _lock.HasAccess(tag, identityId, operation, cancellationToken);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Secured_ShouldReturnOnlyTagsWithMatchingUserId()
    {
        // Arrange
        int identityId = 1;
        var tagList = new List<Tag>
        {
            CreateTestTag(Guid.NewGuid(), identityId, "Work"),
            CreateTestTag(Guid.NewGuid(), identityId, "Personal"),
            CreateTestTag(Guid.NewGuid(), 2, "Shopping"),
            CreateTestTag(Guid.NewGuid(), 3, "Travel")
        };

        Mock<DbSet<Tag>> mockDbSet = tagList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockDbSet.Object);

        // Act
        var result = _lock.Secured(identityId).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.All(t => t.UserId == identityId).ShouldBeTrue();
    }

    [Fact]
    public void Secured_WhenNoTagsWithMatchingUserId_ShouldReturnEmptyResult()
    {
        // Arrange
        int identityId = 999; // Non-existent user ID
        var tagList = new List<Tag>
        {
            CreateTestTag(Guid.NewGuid(), 1, "Work"),
            CreateTestTag(Guid.NewGuid(), 2, "Shopping"),
            CreateTestTag(Guid.NewGuid(), 3, "Travel")
        };

        Mock<DbSet<Tag>> mockDbSet = tagList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockDbSet.Object);

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
        var tagList = new List<Tag>
        {
            CreateTestTag(Guid.NewGuid(), identityId, "Work"),
            CreateTestTag(Guid.NewGuid(), identityId, "Personal"),
            CreateTestTag(Guid.NewGuid(), identityId, "Personal Finance"),
            CreateTestTag(Guid.NewGuid(), 2, "Personal")
        };

        Mock<DbSet<Tag>> mockDbSet = tagList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockDbSet.Object);

        // Act
        var result = _lock.Secured(identityId)
            .Where(t => t.Name.Contains("Personal"))
            .ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.All(t => t.UserId == identityId).ShouldBeTrue();
        result.All(t => t.Name.Contains("Personal")).ShouldBeTrue();
    }

    [Fact]
    public void Secured_ShouldAllowOrderingAndLimiting()
    {
        // Arrange
        int identityId = 1;
        var tagList = new List<Tag>
        {
            CreateTestTag(Guid.NewGuid(), identityId, "Zebra"),
            CreateTestTag(Guid.NewGuid(), identityId, "Apple"),
            CreateTestTag(Guid.NewGuid(), identityId, "Banana"),
            CreateTestTag(Guid.NewGuid(), 2, "Cherry")
        };

        Mock<DbSet<Tag>> mockDbSet = tagList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<Tag>()).Returns(mockDbSet.Object);

        // Act
        var result = _lock.Secured(identityId)
            .OrderBy(t => t.Name)
            .Take(2)
            .ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result[0].Name.ShouldBe("Apple");
        result[1].Name.ShouldBe("Banana");
    }
}
