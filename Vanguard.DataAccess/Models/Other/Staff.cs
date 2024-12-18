namespace Vanguard.DataAccess.Models;
public partial class Staff
{
    public int Staffid { get; set; }
    public int Userid { get; set; }
    public DateTime? Dateempl { get; set; }

    public virtual ICollection<StaffDocument> Staffdocuments { get; set; } = new List<StaffDocument>();
    public virtual User User { get; set; } = null!;
}
