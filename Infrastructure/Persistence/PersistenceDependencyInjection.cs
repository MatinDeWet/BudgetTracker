using System.Reflection;
using Application.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common.DatabaseSchemas;
using Persistence.Common.DomainEvents;
using Persistence.Common.Repository;
using Persistence.Data.Context;
using Persistence.Data.Inteceptors;

namespace Persistence;
public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddSecuredRepositories(this IServiceCollection services, Type assemplyPointer)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemplyPointer.Assembly)
            .AddClasses((classes) => classes.AssignableToAny(typeof(ISecureQuery), typeof(ISecureCommand)), publicOnly: false)
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(assemplyPointer.Assembly)
            .AddClasses((classes) => classes.AssignableToAny(typeof(IProtected)), publicOnly: false)
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, bool IsDevelopment)
    {
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<BudgetContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    opt =>
                    {
                        opt.MigrationsAssembly(typeof(BudgetContext).GetTypeInfo().Assembly.GetName().Name);
                        opt.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default);
                    });

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                if (IsDevelopment)
                {
                    options.EnableSensitiveDataLogging();
                }
            });

        return services;
    }
}
