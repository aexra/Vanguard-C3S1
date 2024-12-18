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
        var result = await _context.Database.SqlQuery<Contract>(@$"
            SELECT * FROM Contracts;
        ").ToListAsync();

        return Ok(result);
    }

    [HttpGet("user={ownerid}")]
    public async Task<IActionResult> GetContractsByUser([FromRoute] string ownerid)
    {
        var result = await _context.Database.SqlQuery<Contract>(@$"
            SELECT * FROM Contracts
            WHERE ownerid = {ownerid};
        ").ToListAsync();

        return Ok(result);
    }

    [HttpGet("organization={organizationid}")]
    public async Task<IActionResult> GetContractsByOrganization([FromRoute] string organizationid)
    {
        var result = await _context.Database.SqlQuery<Contract>(@$"
            SELECT * FROM Contracts
            WHERE organizationid = {organizationid};
        ").ToListAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContracts([FromBody] List<Contract> contracts)
    {
        if (contracts.Count < 1) return BadRequest();

        var sql = @$"INSERT INTO Contracts (ContractId, OrganizationId, OwnerId, IsLegalEntity, SignDate, Address, Price, Comment) VALUES ";

        foreach (var c in contracts)
        {
            sql += $"\n({c.ContractId},{c.OrganizationId},{c.OwnerId},{c.IsLegalEntity},{c.SignDate},{c.Address},{c.Price},{c.Comment}),";
        }

        sql = sql[..^1];

        var result = _context.Database.SqlQuery<string>($@"{sql}");

        return Ok(result);
    }
}
