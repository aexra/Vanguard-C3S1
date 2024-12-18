using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vanguard.DataAccess.Models;
using Vanguard.Web.Data.Contexts;

namespace Vanguard.Web.Controllers.DBControllers;

[ApiController]
[Route("api/contracts")]
public class ContractController : ControllerBase
{
    private readonly MainContext _context;

    public ContractController(MainContext mainContext)
    {
        _context = mainContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetContracts()
    {
        return Ok(await _context.Contracts.ToListAsync());
    }

    [HttpGet("user={ownerid}")]
    public async Task<IActionResult> GetContractsByUser([FromRoute] string ownerid)
    {
        return Ok(await _context.Contracts.Where(c => c.OwnerId == int.Parse(ownerid)).ToListAsync());
    }

    [HttpGet("organization={organizationid}")]
    public async Task<IActionResult> GetContractsByOrganization([FromRoute] string organizationid)
    {
        return Ok(await _context.Contracts.Where(c => c.OrganizationId == int.Parse(organizationid)).ToListAsync());
    }

    [HttpPut]
    public async Task<IActionResult> UpdateContract([FromBody] Contract contract)
    {
        var entity = await _context.Contracts.Where(c => c.ContractId == contract.ContractId).FirstOrDefaultAsync();

        if (entity == null)
        {
            return NotFound();
        }

        entity.Comment = contract.Comment;
        entity.ContractId = contract.ContractId;
        entity.Address = contract.Address;
        entity.OwnerId = contract.OwnerId;
        entity.OrganizationId = contract.OrganizationId;
        entity.IsLegalEntity = contract.IsLegalEntity;
        entity.Price = contract.Price;
        entity.SignDate = contract.SignDate;

        await _context.SaveChangesAsync();

        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContracts([FromBody] List<Contract> contracts)
    {
        foreach (Contract contract in contracts)
        {
            await _context.Contracts.AddAsync(contract);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteContracts([FromBody] int[] ids)
    {
        _context.Contracts.RemoveRange(_context.Contracts.Where(c => ids.Contains(c.ContractId)));
        await _context.SaveChangesAsync();

        return Ok();
    }
}
