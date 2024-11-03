using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vanguard.DataAccess.Data.DTOs;
using Vanguard.Web.Data.Contexts;
using Vanguard.Web.Data.Models;
using Vanguard.Web.Interfaces;
using Vanguard.Web.Services;

namespace Vanguard.Web.Controllers;

[ApiController]
[Route("api/user")]
public class AppUserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly YamlConfigService _yamlConfigService;
    private readonly UserRoleService _userRoleService;
    private readonly IdentityContext _identityContext;

    public AppUserController(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        YamlConfigService yamlConfigService,
        UserRoleService userRoleService,
        IdentityContext identityContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _yamlConfigService = yamlConfigService;
        _userRoleService = userRoleService;
        _identityContext = identityContext;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.Users
            .FirstAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new ProfileInfoDto
        {
            Id = userId,
            UserName = user.UserName,
            Email = user.Email,
        });
    }

    [HttpGet("profile/{id}")]
    [Authorize]
    public async Task<IActionResult> GetProfile([FromRoute] string id)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new ProfileInfoDto
        {
            Id = id,
            UserName = user.UserName,
            Email = user.Email,
        });
    }

    [HttpGet("check")]
    [Authorize]
    public async Task<IActionResult> CheckLogin()
    {
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userManager.Users.ToListAsync());
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAll()
    {
        await _userManager.Users.ExecuteDeleteAsync();
        await _identityContext.SaveChangesAsync();

        return Ok();
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
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

        return Ok(new LoginResponseDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Token = await _tokenService.CreateToken(user),
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
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
                    return Ok(new LoginResponseDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Token = await _tokenService.CreateToken(user)
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
    public async Task<IActionResult> UpdateSelf([FromBody] ProfileInfoDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        if (dto.UserName != null) user.UserName = dto.UserName;
        if (dto.Email != null) user.Email = dto.Email;

        await _identityContext.SaveChangesAsync();

        return Ok(new ProfileInfoDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        });
    }

    [HttpPut("{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser([FromBody] ProfileInfoDto dto, [FromRoute] string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        if (dto.UserName != null) user.UserName = dto.UserName;
        if (dto.Email != null) user.Email = dto.Email;

        await _identityContext.SaveChangesAsync();

        return Ok(new ProfileInfoDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        });
    }
}
