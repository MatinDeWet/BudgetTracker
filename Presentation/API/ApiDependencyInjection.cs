using Application.Common.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Persistence.Data.Context;
using Serilog;

namespace API;

public static class ApiDependencyInjection
{
    public static IHostBuilder AddSerilogSupport(this IHostBuilder webBuilder)
    {
        webBuilder.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

        return webBuilder;
    }

    public static IApplicationBuilder UseSerilogSupport(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }

    public static IServiceCollection AddIdentityPrepration(this IServiceCollection services)
    {
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(AuthConstants.LoginProvider)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<BudgetContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
