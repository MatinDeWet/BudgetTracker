using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using Persistence.Common.Search;
using Shouldly;
using Domain.Common.Search;

namespace Persistence.UnitTests.Tests.Common.Search;
public class SearchableModelConfigurationTests
{
    [Fact]
    public void Configure_ShouldCallConfigureSearchVector()
    {
        // Arrange
        // Create a ModelBuilder with test conventions
        var modelBuilder = new ModelBuilder(new ConventionSet());

        // Act
        var configuration = new TestArticleConfiguration();
        configuration.Configure(modelBuilder.Entity<TestArticle>());

        // Assert - Verify model is configured correctly
        IMutableModel model = modelBuilder.Model;
        IMutableEntityType? entity = model.FindEntityType(typeof(TestArticle));

        entity.ShouldNotBeNull();

        // Check that SearchVector property is properly configured
        IMutableProperty? searchVectorProperty = entity.FindProperty(nameof(TestArticle.SearchVector));
        searchVectorProperty.ShouldNotBeNull();

        // Verify the index was created
        IMutableIndex? index = entity.GetIndexes().FirstOrDefault(i =>
            i.Properties.Any(p => p.Name == nameof(TestArticle.SearchVector)));
        index.ShouldNotBeNull();
    }

    [Fact]
    public void GetSearchLanguage_ShouldReturnEnglishByDefault()
    {
        // Arrange
        var configuration = new TestArticleConfiguration();

        // Act
        string language = GetProtectedMethodValue<string>(configuration, "GetSearchLanguage");

        // Assert
        language.ShouldBe("english");
    }

    [Fact]
    public void GetSearchLanguage_CanBeOverridden()
    {
        // Arrange
        var configuration = new SpanishArticleConfiguration();

        // Act
        string language = GetProtectedMethodValue<string>(configuration, "GetSearchLanguage");

        // Assert
        language.ShouldBe("spanish");
    }

    [Fact]
    public void GetSearchableProperties_ShouldBeImplementedByDerivedClass()
    {
        // Arrange
        var configuration = new TestArticleConfiguration();

        // Act
        Expression<Func<TestArticle, object>> properties =
            GetProtectedMethodValue<Expression<Func<TestArticle, object>>>(
                configuration, "GetSearchableProperties");

        // Assert
        properties.ShouldNotBeNull();

        // Verify the expression type and structure
        var memberInit = properties.Body as NewExpression;
        memberInit.ShouldNotBeNull();
    }

    private T GetProtectedMethodValue<T>(object instance, string methodName)
    {
        MethodInfo method = instance.GetType().GetMethod(methodName,
            BindingFlags.NonPublic | BindingFlags.Instance)
            ?? instance.GetType().BaseType!.GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance)!;

        return (T)method.Invoke(instance, null)!;
    }

    // Test classes for the unit tests
    public class TestArticle : ISearchableModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        // This attribute excludes the property from EF Core mapping
        [NotMapped]
        public NpgsqlTsVector SearchVector { get; set; } = null!;
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TestArticle> Articles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use a mock configuration approach to avoid actual database interaction
            modelBuilder.Entity<TestArticle>()
                .Ignore(a => a.SearchVector); // Alternative approach to [NotMapped]

            modelBuilder.ApplyConfiguration(new TestArticleConfiguration());
        }
    }

    public class TestArticleConfiguration : SearchableModelConfiguration<TestArticle>
    {
        protected override Expression<Func<TestArticle, object>> GetSearchableProperties()
        {
            return a => new { a.Title, a.Content };
        }
    }

    public class SpanishArticleConfiguration : SearchableModelConfiguration<TestArticle>
    {
        protected override string GetSearchLanguage() => "spanish";

        protected override Expression<Func<TestArticle, object>> GetSearchableProperties()
        {
            return a => new { a.Title, a.Content };
        }
    }
}

