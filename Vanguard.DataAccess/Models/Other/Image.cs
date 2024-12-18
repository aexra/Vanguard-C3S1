namespace Vanguard.DataAccess.Models;
public partial class Image
{
    public int ImageId { get; set; }
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public byte[]? Content { get; set; }

    public virtual ICollection<ObjectPhoto> ObjectPhotos { get; set; } = new List<ObjectPhoto>();
    public virtual ICollection<StaffDocumentScan> StaffDocumentScans { get; set; } = new List<StaffDocumentScan>();
}
