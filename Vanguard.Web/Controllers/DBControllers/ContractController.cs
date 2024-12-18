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
            SELECT * FROM Contracts
        ").ToListAsync();

        return Ok(result);
    }

    [HttpGet("user={id}")]
    public async Task<IActionResult> GetContractsByUser([FromRoute] int id)
    {
        var result = await _context.Database.SqlQuery<Contract>(@$"
            SELECT * FROM Contracts
            WHERE OwnerId = {id}
        ").ToListAsync();

        return Ok(result);
    }

    [HttpGet("organization={id}")]
    public async Task<IActionResult> GetContractsByOrganization([FromRoute] int id)
    {
        var result = await _context.Database.SqlQuery<Contract>(@$"
            SELECT * FROM Contracts
            WHERE OrganizationId = {id}
        ").ToListAsync();

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateContract([FromBody] Contract contract)
    {
        var result = await _context.Database.SqlQuery<string>(@$"
            UPDATE Contracts
            SET OrganizationId = {contract.OrganizationId},
                OwnerId = {contract.OwnerId},
                IsLegalEntity = {contract.IsLegalEntity},
                SignDate = {contract.SignDate},
                Address = {contract.Address},
                Price = {contract.Price},
                Comment = {contract.Comment}
            WHERE ContractId = {contract.ContractId}
        ").ToListAsync();

        await _context.SaveChangesAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] Contract c)
    {
        var result = _context.Database.SqlQuery<string>(@$"
            INSERT INTO Contracts (OrganizationId, OwnerId, IsLegalEntity, SignDate, Address, Price, Comment) VALUES
            ({c.OrganizationId},{c.OwnerId},{c.IsLegalEntity},{c.SignDate},{c.Address},{c.Price},{c.Comment})                            
        ");

        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteContracts([FromBody] List<int> ids)
    {
        var sql = @$"
            DELETE FROM Contracts
            WHERE ContractId IN({ string.Join(", ", ids)})
        ";

        var result = _context.Database.SqlQuery<string>(@$"{sql}");

        return Ok(result);
    }
}
