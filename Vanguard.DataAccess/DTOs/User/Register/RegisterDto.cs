using System.ComponentModel.DataAnnotations;

namespace Vanguard.DataAccess.Data.DTOs;
public class RegisterDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
