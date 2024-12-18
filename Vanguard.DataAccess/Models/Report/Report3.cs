using Vanguard.DataAccess.Enums;

namespace Vanguard.DataAccess.Models;
public class Report3
{
    public string Name { get; set; }
    public CrewCallStatus CallType { get; set; }
    public int ContractId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string PhoneNumber { get; set; }
}
