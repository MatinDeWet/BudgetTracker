using Domain.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace API.Endpoints.AuthEndpoints.AuthRegister;

public class AuthRegisterEndpoint(UserManager<ApplicationUser> userManager) : Endpoint<AuthRegisterRequest>
{
    public override void Configure()
    {
        Post("/auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AuthRegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            User = new User
            {
            }
        };

        IdentityResult? result = await userManager.CreateAsync(user, request.Password);

        if (result is null)
        {
            ThrowError("User creation failed!");
        }

        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                AddError(error.Description);
            }

            ThrowIfAnyErrors();
        }

        ThrowIfAnyErrors();
    }
}
