using Application.Common.Repository;
using Domain.Entities;

namespace Application.Repositories.Query;
public interface ITagQueryRepository : ISecureQuery
{
    IQueryable<Tag> Tags { get; }
}
