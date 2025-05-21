using System.Reflection;
using Application.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common.DatabaseSchemas;
using Persistence.Common.Repository;
using Persistence.Data.Context;

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

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, bool IsDevelopment)
    {
        services.AddDbContext<BudgetContext>(options =>
            {
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
