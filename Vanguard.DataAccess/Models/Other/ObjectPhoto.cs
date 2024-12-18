namespace Vanguard.DataAccess.Models;
public partial class ObjectPhoto
{
    public int PhotoId { get; set; }
    public int ContractId { get; set; }
    public int ImageId { get; set; }

    public virtual Contract Contract { get; set; } = null!;
    public virtual Image Image { get; set; } = null!;
}
