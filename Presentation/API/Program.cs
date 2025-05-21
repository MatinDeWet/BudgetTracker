using Application;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data.Context;

namespace API;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Host.AddSerilogSupport();

        builder.Services.AddMessagingSupport(typeof(IApplicationPointer));
        builder.Services.AddIdentityPrepration();
        builder.Services.AddIdentitySupport();

        builder.Services.AddDatabase(builder.Configuration, builder.Environment.IsDevelopment() || builder.Environment.IsStaging());
        builder.Services.AddSecuredRepositories(typeof(IPersistencePointer));


        builder.Services
            .AddAuthenticationJwtBearer(o => o.SigningKey = builder.Configuration["Auth:JWTSigningKey"])
            .AddAuthorization()
            .AddFastEndpoints()
            .SwaggerDocument(o =>
            {
                o.AutoTagPathSegmentIndex = 2;
                o.ShortSchemaNames = true;
            });

        WebApplication app = builder.Build();

        app.UseSerilogSupport();

        app.UseAuthentication()
           .UseAuthorization()
           .UseFastEndpoints()
           .UseSwaggerGen();

        ApplyDbMigrations(app);

        app.Run();
    }

    internal static void ApplyDbMigrations(IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        if (serviceScope.ServiceProvider.GetRequiredService<BudgetContext>().Database.GetPendingMigrations().Any())
        {
            serviceScope.ServiceProvider.GetRequiredService<BudgetContext>().Database.Migrate();
        }
    }
}
