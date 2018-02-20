using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Server.DAL
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarOnlineStatus> CarOnlineStatuses { get; set; }
        public DbSet<CarLockedStatus> CarLockedStatuses { get; set; }
        public DbSet<CarSpeed> CarSpeeds { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}