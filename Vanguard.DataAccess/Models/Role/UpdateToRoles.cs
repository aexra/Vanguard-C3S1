using System.ComponentModel.DataAnnotations;

namespace Vanguard.DataAccess.Models;
public class UpdateToRoles
{
    [Required]
    public string UserId { get; set; }

    public List<string> Add { get; set; }
    public List<string> Remove { get; set; }
}
