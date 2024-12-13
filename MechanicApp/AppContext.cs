using Microsoft.EntityFrameworkCore;
using MechanicApp.Shared;

namespace MechanicApp
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }

        public AppContext(){}

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
    }
}
