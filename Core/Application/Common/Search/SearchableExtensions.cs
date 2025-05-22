using Domain.Common.Search;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Search;
public static class SearchableExtensions
{
    /// <summary>
    /// Searches entities implementing ISearchableModel using PostgreSQL full-text search
    /// </summary>
    /// <typeparam name="T">Entity type that implements ISearchableModel</typeparam>
    /// <param name="queryable">The queryable to filter</param>
    /// <param name="searchTerm">The search term</param>
    /// <param name="language">The language for text search (defaults to English)</param>
    /// <returns>Filtered queryable with entities matching the search term</returns>
    public static IQueryable<T> FullTextSearch<T>(
        this IQueryable<T> queryable,
        string? searchTerm,
        string language = "english")
        where T : class, ISearchableModel
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return queryable;
        }

        // Process search term to handle multiple words and clean input
        searchTerm = ProcessSearchTerm(searchTerm);

        return queryable.Where(e =>
            e.SearchVector.Matches(EF.Functions.PlainToTsQuery(language, searchTerm)));
    }

    /// <summary>
    /// Processes the search term to ensure proper formatting for PostgreSQL full-text search
    /// </summary>
    private static string ProcessSearchTerm(string searchTerm)
    {
        searchTerm = searchTerm.Trim();

        return searchTerm;
    }
}
