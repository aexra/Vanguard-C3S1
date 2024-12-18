using System.Security.Claims;
using IdentityBase.Interfaces;
using IdentityBase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vanguard.DataAccess.Models;
using Vanguard.Web.Data.Contexts;

namespace Vanguard.Web.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService<User> _tokenService;
    private readonly UserRoleService<User> _userRoleService;
    private readonly MainContext _mainContext;

    public UserController(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager,
        ITokenService<User> tokenService,
        UserRoleService<User> userRoleService,
        MainContext mainContext
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userRoleService = userRoleService;
        _mainContext = mainContext;
    }

    [HttpGet("check")]
    [Authorize]
    public async Task<IActionResult> CheckLogin()
    {
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAll()
    {
        foreach (var user in await _userManager.Users.ToListAsync())
        {
            await _userManager.DeleteAsync(user);
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{username}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteByName([FromRoute] string username)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

        if (user == null)
        {
            return NotFound();
        }

        await _userManager.DeleteAsync(user);

        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userManager.Users.ToListAsync());
    }

    [HttpGet]
    [Route("{username}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByName([FromRoute] string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("profile/{id}")]
    [Authorize]
    public async Task<IActionResult> GetProfile([FromRoute] string id)
    {
        var user = await _userManager.Users
            .FirstAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserProfile
        {
            UserId = user.Id,
            UserName = user.UserName,
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == dto.UserName);

        if (user == null)
        {
            return Unauthorized("Invalid username");
        }

        var result = _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

        if (!result.IsCompletedSuccessfully) return Unauthorized("Username not found and/or password incorrect");

        return Ok(new LoginUserResult
        {
            UserId = user.Id,
            UserName = user.UserName,
            Token = await _tokenService.CreateToken(user),
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return BadRequest();
        }

        var user = await _userManager.Users
            .FirstAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserProfile
        {
            UserId = user.Id,
            UserName = user.UserName,
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                UserName = dto.UserName,
            };

            var createdUser = await _userManager.CreateAsync(user, dto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    return Ok(new LoginUserResult
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Token = await _tokenService.CreateToken(user),
                    });
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateSelf([FromBody] UpdateUser dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.Users
            .FirstAsync(u => u.Id == userId);

        if (dto.UserName != null) user.UserName = dto.UserName;

        await _mainContext.SaveChangesAsync();

        return Ok(new UserProfile
        {
            UserId = user.Id,
            UserName = user.UserName,
        });
    }

    [HttpPut("{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUser dto, [FromRoute] string userId)
    {
        var user = await _userManager.Users
            .FirstAsync(u => u.Id == userId);

        if (dto.UserName != null) user.UserName = dto.UserName;

        await _mainContext.SaveChangesAsync();

        return Ok(new UserProfile
        {
            UserId = user.Id,
            UserName = user.UserName,
        });
    }
}
