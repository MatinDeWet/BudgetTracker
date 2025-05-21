using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence.Common.Repository;

namespace Persistence.UnitTests.Tests.Common.Repository;

// Sample entity for testing
public class TestEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

// Concrete implementation of the abstract Lock<T> class for testing
public class TestEntityLock : Lock<TestEntity>
{
    private readonly List<TestEntity> _entities;

    public TestEntityLock(List<TestEntity> entities)
    {
        _entities = entities;
    }

    public override Task<bool> HasAccess(
        TestEntity obj,
        int identityId,
        RepositoryOperationEnum operation,
        CancellationToken cancellationToken)
    {
        // For testing purposes, allow access if identityId is positive
        return Task.FromResult(identityId > 0);
    }

    public override IQueryable<TestEntity> Secured(int identityId)
    {
        // For testing purposes, return all entities if identityId is positive
        if (identityId > 0)
        {
            return _entities.AsQueryable();
        }

        return Enumerable.Empty<TestEntity>().AsQueryable();
    }
}

public class LockTests
{
    private readonly TestEntityLock _lock;
    private readonly List<TestEntity> _entities;

    public LockTests()
    {
        _entities =
        [
            new TestEntity { Id = 1, Name = "Entity 1" },
            new TestEntity { Id = 2, Name = "Entity 2" }
        ];

        _lock = new TestEntityLock(_entities);
    }

    [Fact]
    public void IsMatch_WithMatchingType_ReturnsTrue()
    {
        // Act
        bool result = _lock.IsMatch(typeof(TestEntity));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsMatch_WithDerivedType_ReturnsTrue()
    {
        // Arrange
        Type derivedType = typeof(DerivedTestEntity);

        // Act
        bool result = _lock.IsMatch(derivedType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsMatch_WithUnrelatedType_ReturnsFalse()
    {
        // Act
        bool result = _lock.IsMatch(typeof(string));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task HasAccess_WithPositiveIdentityId_ReturnsTrue()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Test" };

        // Act
        bool result = await _lock.HasAccess(
            entity,
            1, // positive identityId
            RepositoryOperationEnum.Read,
            CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HasAccess_WithNonPositiveIdentityId_ReturnsFalse()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Test" };

        // Act
        bool result = await _lock.HasAccess(
            entity,
            0, // non-positive identityId
            RepositoryOperationEnum.Read,
            CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Secured_WithPositiveIdentityId_ReturnsAllEntities()
    {
        // Act
        IQueryable<TestEntity> result = _lock.Secured(1);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, e => e.Id == 1);
        Assert.Contains(result, e => e.Id == 2);
    }

    [Fact]
    public void Secured_WithNonPositiveIdentityId_ReturnsEmptyQueryable()
    {
        // Act
        IQueryable<TestEntity> result = _lock.Secured(0);

        // Assert
        Assert.Empty(result);
    }

    // Helper derived class for testing IsMatch with derived types
    private sealed class DerivedTestEntity : TestEntity { }
}
