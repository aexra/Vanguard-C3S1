using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vanguard.DataAccess.Models;
using Vanguard.Web.Data.Contexts;

namespace Vanguard.Web.Controllers.DBControllers;

[ApiController]
[Route("api/crews")]
public class CrewController : ControllerBase
{
    private readonly MainContext _context;

    public CrewController(MainContext context)
    {
        _context = context;
    }

    [HttpGet("{crewid}/calls")]
    public async Task<IActionResult> GetCrewCalls([FromRoute] string crewid)
    {
        var result = await _context.Database.SqlQuery<Contract>(@$"
            SELECT * FROM CrewCalls
            WHERE crewid = {crewid}
        ").ToListAsync();

        return Ok(result);
    }

    [HttpPost("{crewid}/calls")]
    public async Task<IActionResult> CreateCrewCalls([FromRoute] string crewid, [FromBody] List<CrewCall> calls)
    {
        if (calls.Count < 1) return BadRequest();

        var sql = @$"INSERT INTO CrewCalls (CrewId, ContractId, CallType, DateTime) VALUES ";

        foreach (var call in calls)
        {
            sql += $"\n({call.CrewId},{call.ContractId},{call.CallType},{call.DateTime}),";
        }

        sql = sql[..^1];

        var result = _context.Database.SqlQuery<string>($@"{sql}");

        return Ok(result);
    }
}
