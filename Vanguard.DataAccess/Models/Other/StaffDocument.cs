namespace Vanguard.DataAccess.Models;
public partial class StaffDocument
{
    public int DocumentId { get; set; }
    public int StaffId { get; set; }

    public virtual Staff Staff { get; set; } = null!;
    public virtual ICollection<StaffDocumentScan> StaffDocumentScans { get; set; } = new List<StaffDocumentScan>();
}
