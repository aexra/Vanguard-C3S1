namespace Vanguard.DataAccess.Models;
public partial class StaffDocumentScan
{
    public int ScanId { get; set; }
    public int DocumentId { get; set; }
    public int ImageId { get; set; }

    public virtual StaffDocument Document { get; set; } = null!;
    public virtual Image Image { get; set; } = null!;
}
