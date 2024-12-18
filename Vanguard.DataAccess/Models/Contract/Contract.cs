using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vanguard.DataAccess.Models;

[Table("Contracts")]
public class Contract
{
    [Key]
    public int ContractId { get; set; }
    public int? OrganizationId { get; set; }
    public int? OwnerId { get; set; }

    public bool IsLegalEntity { get; set; }
    public DateTime SignDate { get; set; }
    public string Address { get; set; }
    public double Price { get; set; }
    public string Comment { get; set; }
}
