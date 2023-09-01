using System.Threading.Tasks;
using W3.Domain.Entities.UserDetails;

namespace W3.Domain.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(Users user);
    }
}
