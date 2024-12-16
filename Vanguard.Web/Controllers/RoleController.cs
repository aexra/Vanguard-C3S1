using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vanguard.DataAccess.Models;
using Vanguard.Web.Data.Models;

namespace Vanguard.Web.Controllers;

[Route("api/roles")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRoles([FromBody] RolesList dto)
    {
        foreach (var roleName in dto.Roles)
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }

        return Ok(_roleManager.Roles);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRoles([FromBody] RolesList dto)
    {
        foreach (var roleName in dto.Roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }

        return Ok(_roleManager.Roles);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("{username}")]
    public async Task<IActionResult> GetRoles([FromRoute] string username)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(_userManager.GetRolesAsync(user).Result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateToRoles([FromBody] UpdateToRoles dto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == dto.UserId);

        if (user == null)
        {
            return NotFound($"User \"{dto.UserId}\" not found");
        }

        foreach (var roleName in dto.Remove)
        {
            if (await _roleManager.FindByNameAsync(roleName) != null)
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);
            }
        }

        foreach (var roleName in dto.Add)
        {
            if (await _roleManager.FindByNameAsync(roleName) != null)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        return Ok(new
        {
            user.Id,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        });
    }
}
