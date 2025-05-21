using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.IdentitySupport;
using Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Common.Repository;

namespace Persistence.UnitTests.Tests.Common.Repository;
// Test entity for JudgedCommands
public class TestCommandEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class JudgedCommandsTests
{
    private readonly Mock<DbContext> _mockContext;
    private readonly Mock<IIdentityInfo> _mockIdentityInfo;
    private readonly Mock<IProtected<TestCommandEntity>> _mockProtected;
    private readonly List<IProtected> _protectedEntities;
    private readonly JudgedCommands<DbContext> _judgedCommands;

    public JudgedCommandsTests()
    {
        // Mock the DbContext
        _mockContext = new Mock<DbContext>();

        // Setup identity info mock
        _mockIdentityInfo = new Mock<IIdentityInfo>();

        // Setup protected entity mock
        _mockProtected = new Mock<IProtected<TestCommandEntity>>();
        _mockProtected.Setup(p => p.IsMatch(typeof(TestCommandEntity))).Returns(true);

        _protectedEntities = [_mockProtected.Object];

        // Create JudgedCommands instance to test
        _judgedCommands = new JudgedCommands<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            _protectedEntities);
    }

    #region Insert Tests

    [Fact]
    public async Task InsertAsync_WithSuperAdminRole_AddsEntityToContext()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);
        var entity = new TestCommandEntity { Id = 1, Name = "Test Entity" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Add(It.IsAny<TestCommandEntity>())).Verifiable();

        // Act
        await _judgedCommands.InsertAsync(entity, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Add(entity), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_WithPersistImmediately_SavesChanges()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);
        var entity = new TestCommandEntity { Id = 2, Name = "Persisted Entity" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Add(It.IsAny<TestCommandEntity>())).Verifiable();
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();

        // Act
        await _judgedCommands.InsertAsync(entity, true, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Add(entity), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_WithAccessGranted_AddsEntityToContext()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        _mockIdentityInfo.Setup(i => i.GetIdentityId()).Returns(123);

        _mockProtected
            .Setup(p => p.HasAccess(
                It.IsAny<TestCommandEntity>(),
                It.IsAny<int>(),
                It.IsAny<RepositoryOperationEnum>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var entity = new TestCommandEntity { Id = 3, Name = "Regular User Entity" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Add(It.IsAny<TestCommandEntity>())).Verifiable();

        // Act
        await _judgedCommands.InsertAsync(entity, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Add(entity), Times.Once);

        // Verify HasAccess was called with the correct parameters
        _mockProtected.Verify(p => p.HasAccess(
            entity,
            123,
            RepositoryOperationEnum.Insert,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_WithAccessDenied_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        _mockIdentityInfo.Setup(i => i.GetIdentityId()).Returns(123);

        _mockProtected
            .Setup(p => p.HasAccess(
                It.IsAny<TestCommandEntity>(),
                It.IsAny<int>(),
                It.IsAny<RepositoryOperationEnum>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var entity = new TestCommandEntity { Id = 4, Name = "Unauthorized Entity" };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _judgedCommands.InsertAsync(entity, CancellationToken.None));

        // Verify Add was never called
        _mockContext.Verify(c => c.Add(It.IsAny<TestCommandEntity>()), Times.Never);
    }

    [Fact]
    public async Task InsertAsync_WithNoProtectionForEntity_AddsEntityToContext()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);

        // Create JudgedCommands with empty protections list
        var judgedCommandsWithoutProtection = new JudgedCommands<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            []);

        var entity = new TestCommandEntity { Id = 5, Name = "Unprotected Entity" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Add(It.IsAny<TestCommandEntity>())).Verifiable();

        // Act
        await judgedCommandsWithoutProtection.InsertAsync(entity, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Add(entity), Times.Once);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task UpdateAsync_WithSuperAdminRole_UpdatesEntityInContext()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);
        var entity = new TestCommandEntity { Id = 7, Name = "Entity to Update" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Update(It.IsAny<TestCommandEntity>())).Verifiable();

        // Act
        await _judgedCommands.UpdateAsync(entity, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Update(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithPersistImmediately_SavesChanges()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);
        var entity = new TestCommandEntity { Id = 8, Name = "Persisted Update Entity" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Update(It.IsAny<TestCommandEntity>())).Verifiable();
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();

        // Act
        await _judgedCommands.UpdateAsync(entity, true, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Update(entity), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithAccessDenied_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        _mockIdentityInfo.Setup(i => i.GetIdentityId()).Returns(123);

        _mockProtected
            .Setup(p => p.HasAccess(
                It.IsAny<TestCommandEntity>(),
                It.IsAny<int>(),
                It.IsAny<RepositoryOperationEnum>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var entity = new TestCommandEntity { Id = 9, Name = "Unauthorized Update Entity" };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _judgedCommands.UpdateAsync(entity, CancellationToken.None));

        // Verify Update was never called
        _mockContext.Verify(c => c.Update(It.IsAny<TestCommandEntity>()), Times.Never);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task DeleteAsync_WithSuperAdminRole_RemovesEntityFromContext()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);
        var entity = new TestCommandEntity { Id = 10, Name = "Entity to Delete" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Remove(It.IsAny<TestCommandEntity>())).Verifiable();

        // Act
        await _judgedCommands.DeleteAsync(entity, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Remove(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithPersistImmediately_SavesChanges()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);
        var entity = new TestCommandEntity { Id = 11, Name = "Persisted Delete Entity" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Remove(It.IsAny<TestCommandEntity>())).Verifiable();
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();

        // Act
        await _judgedCommands.DeleteAsync(entity, true, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Remove(entity), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithAccessDenied_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        _mockIdentityInfo.Setup(i => i.GetIdentityId()).Returns(123);

        _mockProtected
            .Setup(p => p.HasAccess(
                It.IsAny<TestCommandEntity>(),
                It.IsAny<int>(),
                It.IsAny<RepositoryOperationEnum>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var entity = new TestCommandEntity { Id = 12, Name = "Unauthorized Delete Entity" };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _judgedCommands.DeleteAsync(entity, CancellationToken.None));

        // Verify Remove was never called
        _mockContext.Verify(c => c.Remove(It.IsAny<TestCommandEntity>()), Times.Never);
    }

    #endregion

    #region SaveAsync Tests

    [Fact]
    public async Task SaveAsync_CallsSaveChangesAsyncOnContext()
    {
        // Arrange
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();

        // Act
        await _judgedCommands.SaveAsync(CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region HasAccess (Private Method) Tests via Public Methods

    [Fact]
    public async Task HasAccess_WithSuperAdmin_BypassesProtectionCheck()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(true);

        var entity = new TestCommandEntity { Id = 14, Name = "SuperAdmin Bypass Test" };

        // Setup the protected mock to throw if called - to verify it's bypassed
        _mockProtected
            .Setup(p => p.HasAccess(
                It.IsAny<TestCommandEntity>(),
                It.IsAny<int>(),
                It.IsAny<RepositoryOperationEnum>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("This should not be called for SuperAdmin"));

        // Setup mock expectations
        _mockContext.Setup(c => c.Add(It.IsAny<TestCommandEntity>())).Verifiable();

        // Act - this should not throw if SuperAdmin bypasses the protection check
        await _judgedCommands.InsertAsync(entity, CancellationToken.None);

        // Verify Add was called
        _mockContext.Verify(c => c.Add(entity), Times.Once);

        // Verify the HasAccess method was never called
        _mockProtected.Verify(p => p.HasAccess(
            It.IsAny<TestCommandEntity>(),
            It.IsAny<int>(),
            It.IsAny<RepositoryOperationEnum>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HasAccess_WithNonMatchingProtection_ReturnsTrue()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);

        // Setup a protected entity that doesn't match TestCommandEntity
        var nonMatchingProtected = new Mock<IProtected>();
        nonMatchingProtected.Setup(p => p.IsMatch(typeof(TestCommandEntity))).Returns(false);

        var judgedCommandsWithNonMatchingProtection = new JudgedCommands<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            [nonMatchingProtected.Object]);

        var entity = new TestCommandEntity { Id = 15, Name = "Non-Matching Protection Entity" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Add(It.IsAny<TestCommandEntity>())).Verifiable();

        // Act
        await judgedCommandsWithNonMatchingProtection.InsertAsync(entity, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Add(entity), Times.Once);
    }

    [Fact]
    public async Task HasAccess_WithMultipleProtectionCandidates_UsesFirstMatch()
    {
        // Arrange
        _mockIdentityInfo.Setup(i => i.HasRole(ApplicationRoleEnum.SuperAdmin)).Returns(false);
        _mockIdentityInfo.Setup(i => i.GetIdentityId()).Returns(123);

        // First protection will grant access
        _mockProtected
            .Setup(p => p.HasAccess(
                It.IsAny<TestCommandEntity>(),
                It.IsAny<int>(),
                It.IsAny<RepositoryOperationEnum>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Second protection would deny access, but should never be called
        var mockProtected2 = new Mock<IProtected<TestCommandEntity>>();
        mockProtected2.Setup(p => p.IsMatch(typeof(TestCommandEntity))).Returns(true);
        mockProtected2
            .Setup(p => p.HasAccess(
                It.IsAny<TestCommandEntity>(),
                It.IsAny<int>(),
                It.IsAny<RepositoryOperationEnum>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var judgedCommandsWithMultipleProtection = new JudgedCommands<DbContext>(
            _mockContext.Object,
            _mockIdentityInfo.Object,
            [_mockProtected.Object, mockProtected2.Object]);

        var entity = new TestCommandEntity { Id = 16, Name = "Multiple Protection Entity" };

        // Setup mock expectations
        _mockContext.Setup(c => c.Add(It.IsAny<TestCommandEntity>())).Verifiable();

        // Act
        await judgedCommandsWithMultipleProtection.InsertAsync(entity, CancellationToken.None);

        // Assert
        _mockContext.Verify(c => c.Add(entity), Times.Once);

        // Verify only the first protected entity's HasAccess method was called
        _mockProtected.Verify(p => p.HasAccess(
            entity,
            123,
            RepositoryOperationEnum.Insert,
            It.IsAny<CancellationToken>()), Times.Once);

        mockProtected2.Verify(p => p.HasAccess(
            It.IsAny<TestCommandEntity>(),
            It.IsAny<int>(),
            It.IsAny<RepositoryOperationEnum>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion
}
