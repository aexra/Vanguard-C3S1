namespace Vanguard.DataAccess.Models;
public partial class File
{
    public int FileId { get; set; }
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public byte[]? Content { get; set; }

    public virtual ICollection<ObjectPlan> ObjectPlans { get; set; } = new List<ObjectPlan>();
}
