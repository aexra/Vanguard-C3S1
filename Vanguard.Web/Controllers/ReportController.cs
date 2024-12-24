using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vanguard.DataAccess.Models;
using Vanguard.Web.Data.Contexts;

namespace Vanguard.Web.Controllers;

[ApiController]
[Route("api/report")]
public class ReportController : ControllerBase
{
    private readonly MainContext _context;

    public ReportController(MainContext context)
    {
        _context = context;
    }

    [HttpGet("1")]
    public async Task<IActionResult> GetReport1()
    {
        var result = await _context.Database.SqlQuery<Report1>(@$"
            CREATE TEMP TABLE IF NOT EXISTS OrgsOwners AS
            select organizationid, name, type, ownerid from organizations;

            CREATE TEMP TABLE IF NOT EXISTS OrgsAlarmsCount AS
            select OrganizationId, count(*) from contracts c
            join alarms a on a.ContractId = c.ContractId
            group by c.ContractId;

            CREATE TEMP TABLE IF NOT EXISTS Report1 AS
            select oo.name, oo.type, sum(oac.count), avg(oac.count) from OrgsOwners oo
            join OrgsAlarmsCount oac on oac.OrganizationId = oo.OrganizationId
            group by oo.organizationid, oo.name, oo.type;

            SELECT * FROM Report1;
        ").ToListAsync();

        return Ok(result);
    }

    [HttpGet("2")]
    public async Task<IActionResult> GetReport2()
    {
        var result = await _context.Database.SqlQuery<Report2>(@$"
            DROP TABLE IF EXISTS OrganizationsProfit;
            DROP TABLE IF EXISTS OrganizationsSecurity;
            DROP TABLE IF EXISTS Report2;

            CREATE TEMP TABLE IF NOT EXISTS OrganizationsProfit AS
            select * from (
	            select o.organizationid, sum(price) from (
		            select * from organizations o
		            where o.type = 'Commercial'
	            ) o
	            join contracts c on c.organizationid = o.organizationid
	            where c.isLegalEntity = 'true'
	            group by o.organizationid
            ) g
            order by g.sum desc;

            CREATE TEMP TABLE IF NOT EXISTS OrganizationsSecurity AS
            SELECT g.organizationid, COUNT(a.alarmid) FROM (
	            SELECT o.organizationid, c.contractid FROM (
		            SELECT o.organizationid FROM organizations o
		            WHERE o.type = 'Commercial'
	            ) o
	            JOIN contracts c ON c.organizationid = o.organizationid
	            WHERE c.isLegalEntity = 'true'
            ) g
            JOIN alarms a ON a.contractid = g.contractid
            GROUP BY g.organizationid
            ORDER BY COUNT(a.alarmid);

            CREATE TEMP TABLE IF NOT EXISTS Report2 AS
            SELECT o.name, sum, count FROM OrganizationsProfit op
            JOIN OrganizationsSecurity os ON op.organizationid = os.organizationid
            JOIN organizations o on o.organizationid = op.organizationid
            ORDER BY sum desc, count
            LIMIT 5;

            SELECT * FROM Report2;
        ").ToListAsync();

        return Ok(result);
    }

    [HttpGet("3")]
    public async Task<IActionResult> GetReport3()
    {
        var result = await _context.Database.SqlQuery<Report3>(@$"
            select c.name, cc.calltype, cc.contractid, firstname, lastname, middlename, phonenumber from crews c
            join crewmembers cm on cm.memberid = c.leaderid
            join users u on cm.userid = u.userid
            join crewcalls cc on cc.crewid = c.crewid
        ").ToListAsync();

        return Ok(result);
    }
}
