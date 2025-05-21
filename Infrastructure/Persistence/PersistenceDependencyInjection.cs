using Application.Common.Repository;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common.Repository;

namespace Persistence;
public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddSecuredRepositories(this IServiceCollection services, Type assemplyPointer)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemplyPointer.Assembly)
            .AddClasses((classes) => classes.AssignableToAny(typeof(ISecureQuery), typeof(ISecureCommand)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assemplyPointer.Assembly)
            .AddClasses((classes) => classes.AssignableToAny(typeof(IProtected)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
