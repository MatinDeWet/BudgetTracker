using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.Context;
public class BudgetContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public BudgetContext() { }

    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(BudgetContext).Assembly);

        base.OnModelCreating(builder);
    }
}
