using BackgroundServiceApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dataConnect.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
              
        }
        
        public DbSet<ThongTinNiemYet> ThongTinNiemYet { get; set; }
    }
}
