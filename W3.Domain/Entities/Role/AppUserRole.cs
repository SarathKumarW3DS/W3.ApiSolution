using Microsoft.AspNetCore.Identity;
using W3.Domain.Entities.UserDetails;
namespace W3.Domain.Entities.Role
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public Users User { get; set; }
        public AppRole Role { get; set; }
    }
}
