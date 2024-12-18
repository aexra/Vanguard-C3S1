namespace Vanguard.DataAccess.Models;
public partial class Owner
{
    public int OwnerId { get; set; }
    public int UserId { get; set; }
    public int? OrganizationId { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public virtual Organization? Organization { get; set; }
    public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    public virtual User User { get; set; } = null!;
}
