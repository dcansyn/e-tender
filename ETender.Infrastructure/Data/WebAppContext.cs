using ETender.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ETender.Infrastructure.Data
{
    public class WebAppContext : IdentityDbContext<AppUser>
    {
        public WebAppContext(DbContextOptions<WebAppContext> options)
            : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }

    }
}
