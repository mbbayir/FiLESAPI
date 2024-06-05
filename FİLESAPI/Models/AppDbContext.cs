using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FİLESAPI.Models;

namespace FİLESAPI.Models
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Folder> Folders { get; set; }
        public DbSet<Fillies> Files { get; set; }
    }
}
