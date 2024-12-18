using IdentityBase.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vanguard.DataAccess.Models;
using Vanguard.Web.Data.Models;

namespace Vanguard.Web.Data.Contexts;

public class MainContext : IdentityContext<User>
{
    public DbSet<Contract> Contracts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=vanguard;Username=postgres;Password=1234;Include Error Detail=True");
    }

    public async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        string[] roles = ["User", "Admin"];

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // ENSURE ROLES
        foreach (var roleName in roles)
        {
            if (!roleManager.Roles.ToList().Exists(r => r.Name == roleName))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }

        // ENSURE ADMIN
        if (!(await Users.ToListAsync()).Exists(u => u.UserName == "root"))
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var userName = "root";
            var password = "Root1_";

            var user = new User()
            {
                UserName = userName
            };

            var createdUser = await userManager.CreateAsync(user, password);

            await userManager.AddToRoleAsync(user, "User");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        SaveChanges();
    }
}
