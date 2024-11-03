using Vanguard.Web.Data.Models;

namespace Vanguard.Web.Interfaces;

public interface ITokenService
{
    public Task<string> CreateToken(User user);
}
