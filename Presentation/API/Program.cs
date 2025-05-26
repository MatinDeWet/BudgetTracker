using API.Common.Filters;
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

        builder.Services.AddCors(options => 
            options.AddPolicy("AllowAngularApp", policy => 
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()));


        builder.Services.AddDatabase(builder.Configuration, builder.Environment.IsDevelopment() || builder.Environment.IsStaging());
        builder.Services.AddSecuredRepositories(typeof(IPersistencePointer));


        builder.Services
            .AddAuthenticationJwtBearer(o => o.SigningKey = builder.Configuration["Auth:JWTSigningKey"])
            .AddAuthorization()
            .AddFastEndpoints()
            .SwaggerDocument(o =>
            {
                o.AutoTagPathSegmentIndex = 1;
                o.ShortSchemaNames = true;
            });

        WebApplication app = builder.Build();

        app.UseSerilogSupport();

        app.UseCors("AllowAngularApp");

        app.UseAuthentication()
           .UseAuthorization()
           .UseFastEndpoints(c =>
           {
               c.Endpoints.Configurator = ep => ep.Options(b => b.AddEndpointFilter<IdentityInfoFilter>());
               c.Endpoints.ShortNames = true;
               c.Endpoints.NameGenerator = ctx =>
               {
                   if (ctx.EndpointType.Name.EndsWith("Endpoint", StringComparison.InvariantCulture))
                   {
                       return ctx.EndpointType.Name[..^"Endpoint".Length];
                   }
                   else
                   {
                       return ctx.EndpointType.Name;
                   }
               };
           })
           .UseSwaggerGen(uiConfig: u => u.ShowOperationIDs());

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
