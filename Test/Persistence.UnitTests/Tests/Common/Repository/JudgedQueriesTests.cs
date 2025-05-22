using Application.Common.IdentitySupport;
using Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Common.Repository;

namespace Persistence.UnitTests.Tests.Common.Repository;
// Test entity for JudgedQueries
public class TestQueryEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class JudgedQueriesTests
{
    private readonly Mock<DbContext> _mockContext;
    private readonly Mock<IIdentityInfo> _mockIdentityInfo;
    private readonly Mock<IProtected<TestQueryEntity>> _mockProtected;
    private readonly List<IProtected> _protectedEntities;
    private readonly JudgedQueries<DbContext> _judgedQueries;
    private readonly Mock<DbSet<TestQueryEntity>> _mockDbSet;
    private readonly List<TestQueryEntity> _testEntities;

    public JudgedQueriesTests()
    {
        // Create test entities for the mock DbSet
        _testEntities =
        [
            new TestQueryEntity { Id = 1, Name = "Entity 1" },
            new TestQueryEntity { Id = 2, Name = "Entity 2" }
        ];

        // Set up queryable mock DbSet
        _mockDbSet = CreateMockDbSet(_testEntities);

        // Mock the DbContext
        _mockContext = new Mock<DbContext>();
        _mockContext.Setup(c => c.Set<TestQueryEntity>())
            .Returns(_mockDbSet.Object);

        // Setup identity info mock
        _mockIdentityInfo = new Mock<IIdentityInfo>();

        // Setup protected entity mock
        _mockProtected = new Mock<IProtected<TestQueryEntity>>();
        _mockProtected.Setup(p => p.IsMatch(typeof(TestQueryEntity))).Returns(true);

        // Create a filtered queryable for the mock protected entity
        IQueryable<TestQueryEntity> filteredEntities = _testEntities.Where(e => e.Id == 1).AsQueryable();
        _mockProtected.Setup(p => p.Secured(It.IsAny<int>()))
            .Returns(filteredEntities);

        _protectedEntities = [_mockProtected.Object];

        // Create JudgedQueries instance to test
        _judgedQueries = new JudgedQueries<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            _protectedEntities);
    }

    [Fact]
    public void Secure_WithSuperAdminRole_ReturnsEntireDbSet()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);

        // Act
        IQueryable<TestQueryEntity> result = _judgedQueries.Secure<TestQueryEntity>();

        // Assert
        Assert.Same(_mockDbSet.Object, result);

        // Verify the context's Set<T>() method was called
        _mockContext.Verify(c => c.Set<TestQueryEntity>(), Times.Once);

        // Verify the protected entity's Secured method was not called
        _mockProtected.Verify(p => p.Secured(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Secure_WithoutSuperAdminRole_UsesProtectedEntityFilter()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        _mockIdentityInfo.Setup(i => i.GetIdentityId()).Returns(123);

        // Act
        var result = _judgedQueries.Secure<TestQueryEntity>().ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);

        // Verify the protected entity's Secured method was called with the correct parameters
        _mockProtected.Verify(p => p.Secured(123), Times.Once);
    }

    [Fact]
    public void Secure_WithNoMatchingProtection_ReturnsEntireDbSet()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);

        // Setup a protected entity that doesn't match TestQueryEntity
        var nonMatchingProtected = new Mock<IProtected>();
        nonMatchingProtected.Setup(p => p.IsMatch(typeof(TestQueryEntity))).Returns(false);

        var judgedQueriesWithNonMatchingProtection = new JudgedQueries<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            [nonMatchingProtected.Object]);

        // Act
        IQueryable<TestQueryEntity> result = judgedQueriesWithNonMatchingProtection.Secure<TestQueryEntity>();

        // Assert
        Assert.Same(_mockDbSet.Object, result);

        // Verify the context's Set<T>() method was called
        _mockContext.Verify(c => c.Set<TestQueryEntity>(), Times.Once);
    }

    [Fact]
    public void Secure_WithEmptyProtectionList_ReturnsEntireDbSet()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);

        var judgedQueriesWithEmptyProtection = new JudgedQueries<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            []);

        // Act
        IQueryable<TestQueryEntity> result = judgedQueriesWithEmptyProtection.Secure<TestQueryEntity>();

        // Assert
        Assert.Same(_mockDbSet.Object, result);

        // Verify the context's Set<T>() method was called
        _mockContext.Verify(c => c.Set<TestQueryEntity>(), Times.Once);
    }

    [Fact]
    public void Secure_WithMultipleProtectionCandidates_UsesFirstMatch()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        _mockIdentityInfo.Setup(i => i.GetIdentityId()).Returns(123);

        // Setup a second protected entity that also matches TestQueryEntity but would filter differently
        var mockProtected2 = new Mock<IProtected<TestQueryEntity>>();
        mockProtected2.Setup(p => p.IsMatch(typeof(TestQueryEntity))).Returns(true);

        // This should never be called because it comes second in the list
        IQueryable<TestQueryEntity> alternateFilteredEntities = _testEntities.Where(e => e.Id == 2).AsQueryable();
        mockProtected2.Setup(p => p.Secured(It.IsAny<int>()))
            .Returns(alternateFilteredEntities);

        var judgedQueriesWithMultipleProtection = new JudgedQueries<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            [_mockProtected.Object, mockProtected2.Object]);

        // Act
        var result = judgedQueriesWithMultipleProtection.Secure<TestQueryEntity>().ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);

        // Verify only the first protected entity's Secured method was called
        _mockProtected.Verify(p => p.Secured(123), Times.Once);
        mockProtected2.Verify(p => p.Secured(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Secure_WithDifferentEntityType_AppliesCorrectProtection()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        _mockIdentityInfo.Setup(i => i.GetIdentityId()).Returns(123);

        // Set up a different entity type
        var otherMockDbSet = new Mock<DbSet<OtherTestEntity>>();
        _mockContext.Setup(c => c.Set<OtherTestEntity>())
            .Returns(otherMockDbSet.Object);

        // Set up protection for that entity type
        var otherEntities = new List<OtherTestEntity>
        {
            new() { Id = 1, Description = "Other 1" }
        };
        var otherProtected = new Mock<IProtected<OtherTestEntity>>();
        otherProtected.Setup(p => p.IsMatch(typeof(OtherTestEntity))).Returns(true);
        otherProtected.Setup(p => p.Secured(It.IsAny<int>()))
            .Returns(otherEntities.AsQueryable());

        var multiProtectionList = new List<IProtected> { _mockProtected.Object, otherProtected.Object };

        var judgedQueriesWithMultiTypes = new JudgedQueries<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            multiProtectionList);

        // Act
        IQueryable<OtherTestEntity> result = judgedQueriesWithMultiTypes.Secure<OtherTestEntity>();

        // Assert
        // Verify the other entity's protected.Secured was called
        otherProtected.Verify(p => p.Secured(123), Times.Once);
    }

    // Helper method to create a mock DbSet<T> that works with LINQ methods
    private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        IQueryable<T> queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        return mockSet;
    }

    // Additional test entity type
    public sealed class OtherTestEntity
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
    }
}
