namespace Vanguard.DataAccess.Models;
public partial class Crew
{
    public int CrewId { get; set; }
    public string Name { get; set; } = null!;
    public int LeaderId { get; set; }

    public virtual ICollection<CrewCall> CrewCalls { get; set; } = new List<CrewCall>();
    public virtual ICollection<CrewMember> CrewMembers { get; set; } = new List<CrewMember>();
    public virtual CrewMember Leader { get; set; } = null!;
}
