using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Common.Search;

namespace Persistence.Common.Search;
/// <summary>
/// Base configuration class for entities implementing ISearchableModel
/// </summary>
/// <typeparam name="TEntity">The entity type implementing ISearchableModel</typeparam>
public abstract class SearchableModelConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, ISearchableModel
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ConfigureSearchVector(builder);
    }

    /// <summary>
    /// Configure the full-text search vector for the entity
    /// </summary>
    protected virtual void ConfigureSearchVector(EntityTypeBuilder<TEntity> builder)
    {
        Expression<Func<TEntity, object>> searchableProperties = GetSearchableProperties();
        string language = GetSearchLanguage();

        builder
            .HasGeneratedTsVectorColumn(
                e => e.SearchVector,
                language,
                searchableProperties
            )
            .HasIndex(e => e.SearchVector)
            .HasMethod("GIN");
    }

    /// <summary>
    /// Get the language to use for full-text search
    /// </summary>
    /// <returns>The language identifier</returns>
    protected virtual string GetSearchLanguage() => "english";

    /// <summary>
    /// Get the properties to include in the full-text search index
    /// </summary>
    /// <returns>Expression representing the properties to search</returns>
    protected abstract Expression<Func<TEntity, object>> GetSearchableProperties();
}
