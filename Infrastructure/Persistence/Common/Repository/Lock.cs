namespace Persistence.Common.Repository;
public abstract class Lock<T> : IProtected<T> where T : class
{
    public abstract IQueryable<T> Secured(int identityId);

    public abstract Task<bool> HasAccess(T obj, int identityId, RepositoryOperationEnum operation, CancellationToken cancellationToken);

    public virtual bool IsMatch(Type t)
    {
        return typeof(T).IsAssignableFrom(t);
    }
}
