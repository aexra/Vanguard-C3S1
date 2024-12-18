namespace Vanguard.DataAccess.Models;
public partial class ObjectPlan
{
    public int PlanId { get; set; }
    public int ContractId { get; set; }
    public int FileId { get; set; }

    public virtual Contract Contract { get; set; } = null!;
    public virtual File File { get; set; } = null!;
}
