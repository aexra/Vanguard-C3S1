using System.ComponentModel.DataAnnotations;

namespace Vanguard.DataAccess.Models;
public class LoginUser
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
