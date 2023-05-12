using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TransactionsHandler.Data;

public class UsersContext : IdentityDbContext
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityUser>().ToTable("Users");
    }
}
