using Microsoft.AspNetCore.Identity;
using Vanguard.Web.Data.Models;

namespace Vanguard.Web.Services;

public class UserRoleService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task AddToRolesAsync(User user, params string[] names)
    {
        List<IdentityRole> rolesToAdd = new();

        foreach (var rolename in names)
        {
            var role = await _roleManager.FindByNameAsync(rolename);
            if (role != null) rolesToAdd.Add(role);
        }

        foreach (var role in rolesToAdd)
        {
            var result = await _userManager.AddToRoleAsync(user, role.Name);
        }
    }

    public async Task RemoveFromRolesAsync(User user, params string[] names)
    {
        List<IdentityRole> rolesToDelete = new();

        foreach (var rolename in names)
        {
            var role = await _roleManager.FindByNameAsync(rolename);
            if (role != null) rolesToDelete.Add(role);
        }

        foreach (var role in rolesToDelete)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
        }
    }

    public async Task DeleteRolesAsync(params string[] names)
    {
        foreach (var name in names)
        {
            var role = await _roleManager.FindByNameAsync(name);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }
    }

    public async Task CreateRolesAsync(params string[] names)
    {
        foreach (var name in names)
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = name });
        }
    }
}

