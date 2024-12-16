using System.ComponentModel.DataAnnotations;

namespace Vanguard.DataAccess.Models;
public class RolesList
{
    [Required]
    public List<string> Roles { get; set; }
}
