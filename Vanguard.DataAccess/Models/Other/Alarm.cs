using Vanguard.DataAccess.Enums;

namespace Vanguard.DataAccess.Models;
public partial class Alarm
{
    public int AlarmId { get; set; }
    public AlarmResult? ResultType { get; set; }
    public DateTime? Datetime { get; set; }
    public string? Comment { get; set; }
    public int ContractId { get; set; }

    public virtual Contract Contract { get; set; } = null!;
}
