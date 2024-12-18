namespace Vanguard.DataAccess.Models;
public partial class Organization
{
    public int OrganizationId { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public int OwnerId { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public virtual Owner Owner { get; set; } = null!;
    public virtual ICollection<Owner> Owners { get; set; } = new List<Owner>();
}
