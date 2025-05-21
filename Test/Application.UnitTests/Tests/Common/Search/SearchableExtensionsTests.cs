using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Search;
using Domain.Common.Search;
using Microsoft.EntityFrameworkCore;
using Moq;
using NpgsqlTypes;
using Shouldly;

namespace Application.UnitTests.Tests.Common.Search;
public class SearchableExtensionsTests
{
    [Fact]
    public void FullTextSearch_WithEmptySearchTerm_ShouldReturnOriginalQueryable()
    {
        // Arrange
        IQueryable<TestArticle> testData = GetTestArticles().AsQueryable();

        // Act
        IQueryable<TestArticle> results = testData.FullTextSearch("");

        // Assert
        results.ShouldBeSameAs(testData);
    }

    [Fact]
    public void FullTextSearch_WithNullSearchTerm_ShouldReturnOriginalQueryable()
    {
        // Arrange
        IQueryable<TestArticle> testData = GetTestArticles().AsQueryable();

        // Act
        IQueryable<TestArticle> results = testData.FullTextSearch(null);

        // Assert
        results.ShouldBeSameAs(testData);
    }

    [Fact]
    public void FullTextSearch_WithWhiteSpaceSearchTerm_ShouldReturnOriginalQueryable()
    {
        // Arrange
        IQueryable<TestArticle> testData = GetTestArticles().AsQueryable();

        // Act
        IQueryable<TestArticle> results = testData.FullTextSearch("   ");

        // Assert
        results.ShouldBeSameAs(testData);
    }

    [Fact]
    public void FullTextSearch_WithValidSearchTerm_ShouldApplyFilter()
    {
        // Arrange - Use TestArticleForMocking which doesn't cause EF Core issues
        IQueryable<TestArticleForMocking> mockData = new List<TestArticleForMocking>
        {
            new() { Id = 1, Title = "Test Article 1", Content = "This is test content" },
            new() { Id = 2, Title = "Sample Article", Content = "Another sample content" }
        }.AsQueryable();

        // Create a mock DbSet
        var mockSet = new Mock<DbSet<TestArticleForMocking>>();

        // Set up the mock to return our queryable data
        mockSet.As<IQueryable<TestArticleForMocking>>().Setup(m => m.Provider).Returns(mockData.Provider);
        mockSet.As<IQueryable<TestArticleForMocking>>().Setup(m => m.Expression).Returns(mockData.Expression);
        mockSet.As<IQueryable<TestArticleForMocking>>().Setup(m => m.ElementType).Returns(mockData.ElementType);
        mockSet.As<IQueryable<TestArticleForMocking>>().Setup(m => m.GetEnumerator()).Returns(mockData.GetEnumerator());

        // Set up a mock DbContext
        var mockContext = new Mock<TestMockDbContext>();
        mockContext.Setup(c => c.Articles).Returns(mockSet.Object);

        // Act - build the expression tree (not executing it)
        IQueryable<TestArticleForMocking> query = mockSet.Object.FullTextSearch("test");

        // Assert - since we can't execute it, just verify it's not the same as the original
        query.ShouldNotBeSameAs(mockData);
        // Verify that it's a Where expression
        query.Expression.NodeType.ShouldBe(ExpressionType.Call);
        query.Expression.ToString().ShouldContain("Where");
    }

    [Fact]
    public void ProcessSearchTerm_ShouldTrimInput()
    {
        // Act - Call the private method via reflection
        string result = InvokeProcessSearchTerm("  test term  ");

        // Assert
        result.ShouldBe("test term");
    }

    private string InvokeProcessSearchTerm(string input)
    {
        MethodInfo? method = typeof(SearchableExtensions).GetMethod("ProcessSearchTerm",
            BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("The method 'ProcessSearchTerm' could not be found.");

        return (string)method.Invoke(null, [input])!;
    }

    private List<TestArticle> GetTestArticles()
    {
        return
        [
            new() { Id = 1, Title = "Test Article 1", Content = "This is test content" },
            new() { Id = 2, Title = "Sample Article", Content = "Another sample content" }
        ];
    }

    // Test entity and DbContext classes
    public class TestArticle : ISearchableModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        [NotMapped] // This attribute excludes the property from EF Core mapping
        public NpgsqlTsVector SearchVector { get; set; } = null!;
    }

    // This version will be used with mocking
    public class TestArticleForMocking : ISearchableModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public NpgsqlTsVector SearchVector { get; set; } = null!;
    }

    // Abstract class for mocking
    public abstract class TestMockDbContext : DbContext
    {
        public virtual DbSet<TestArticleForMocking> Articles { get; set; } = null!;
    }
}
