using NpgsqlTypes;

namespace Domain.Common.Search;
public interface ISearchableModel
{
    NpgsqlTsVector SearchVector { get; set; }
}
