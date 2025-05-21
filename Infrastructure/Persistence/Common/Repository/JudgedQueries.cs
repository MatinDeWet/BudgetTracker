using Application.Common.IdentitySupport;
using Application.Common.Repository;
using Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common.Repository;
public class JudgedQueries<TCtx> : ISecureQuery where TCtx : DbContext
{
    protected readonly TCtx _context;
    protected readonly IIdentityInfo _info;

    private readonly IEnumerable<IProtected> _protection;

    public JudgedQueries(TCtx context, IIdentityInfo info, IEnumerable<IProtected> protection)
    {
        _context = context;
        _info = info;
        _protection = protection;
    }

    public IQueryable<T> Secure<T>() where T : class
    {
        if (_info.HasRole(ApplicationRoleEnum.SuperAdmin))
        {
            return _context.Set<T>();
        }

        if (_protection.FirstOrDefault(x => x.IsMatch(typeof(T))) is IProtected<T> entityLock)
        {
            return entityLock.Secured(_info.GetIdentityId());
        }

        return _context.Set<T>();
    }
}
