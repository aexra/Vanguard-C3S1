namespace Vanguard.DataAccess.Models;
public partial class CrewMember
{
    public int MemberId { get; set; }
    public string Name { get; set; } = null!;
    public int? CrewId { get; set; }
    public int UserId { get; set; }

    public virtual Crew? Crew { get; set; }
    public virtual ICollection<Crew> Crews { get; set; } = new List<Crew>();
    public virtual User User { get; set; } = null!;
}
