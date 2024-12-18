using Vanguard.DataAccess.Enums;

namespace Vanguard.DataAccess.Models;
public partial class CrewCall
{
    public int CallId { get; set; }
    public CrewCallStatus? CallType { get; set; }
    public DateTime? DateTime { get; set; }
    public int CrewId { get; set; }
    public int ContractId { get; set; }
}
