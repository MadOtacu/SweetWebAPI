using Microsoft.EntityFrameworkCore;
using SweetWebAPI.Models;

namespace SweetWebAPI
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Stuff> AllStuff { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
