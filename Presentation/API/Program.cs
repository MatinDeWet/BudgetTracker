using Application;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Persistence;

namespace API;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Host.AddSerilogSupport();

        //builder.Services.AddMessagingSupport(typeof(IApplicationPointer));
        builder.Services.AddIdentityPrepration();
        builder.Services.AddIdentitySupport();

        builder.Services.AddDatabase(builder.Configuration, builder.Environment.IsDevelopment() || builder.Environment.IsStaging());
        builder.Services.AddSecuredRepositories(typeof(IPersistencePointer));


        builder.Services
            .AddAuthenticationJwtBearer(o => o.SigningKey = builder.Configuration["Auth:JWTSigningKey"])
            .AddAuthorization()
            .AddFastEndpoints()
            .SwaggerDocument(o => o.AutoTagPathSegmentIndex = 2);

        WebApplication app = builder.Build();

        app.UseSerilogSupport();

        app.UseAuthentication()
           .UseAuthorization()
           .UseFastEndpoints()
           .UseSwaggerGen();

        app.Run();
    }
}
