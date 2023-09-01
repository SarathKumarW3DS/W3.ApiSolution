using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using W3.Domain.Entities.Role;

namespace W3.Domain.Entities.UserDetails
{
    public class Users:IdentityUser<int>
    {
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string address { get; set; }
        public string alternateMobile { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
