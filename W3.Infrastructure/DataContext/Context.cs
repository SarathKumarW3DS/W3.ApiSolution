using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using W3.Domain.Entities.Role;
using W3.Domain.Entities.SideMenu;
using W3.Domain.Entities.UserDetails;

namespace W3.Infrastructure.DataContext
{
    public class Context : IdentityDbContext<Users, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<Users> users { get; set; }
        public DbSet<sidemenu> sidemenus { get; set; }
        public DbSet<SubMenu> subMenus { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Users>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User).HasForeignKey(ur => ur.UserId).IsRequired();
            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
        }
    }
}
