using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using W3.Domain.Entities.Role;

namespace W3.Infrastructure.Seeds
{
    public class UserRoles
    {
        public static async Task SeedUsers(RoleManager<AppRole> roleManager)
        {
            var roles = new List<AppRole>
            {
                new AppRole{Name="SO"},
                new AppRole{Name="OE"},
                new AppRole{Name="HR"},
                new AppRole{Name="Admin"}
            };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

        }
    }
}
