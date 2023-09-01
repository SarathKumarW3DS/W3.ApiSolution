using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace W3.Domain.Entities.Role
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
