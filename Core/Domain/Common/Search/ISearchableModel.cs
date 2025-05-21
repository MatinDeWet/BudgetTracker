using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpgsqlTypes;

namespace Domain.Common.Search;
public interface ISearchableModel
{
    NpgsqlTsVector SearchVector { get; set; }
}
