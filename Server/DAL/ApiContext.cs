using Microsoft.EntityFrameworkCore;
using Shared.Models.Write;
using Shared.Models.Read;
namespace Server.DAL
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarReadNull> CarsReadNull { get; set; }
        public DbSet<CarOnlineStatus> CarOnlineStatuses { get; set; }
        public DbSet<CarOnlineStatusRead> CarOnlineStatusesRead { get; set; }
        public DbSet<CarLockedStatus> CarLockedStatuses { get; set; }
        public DbSet<CarLockedStatusRead> CarLockedStatusesRead { get; set; }
        public DbSet<CarSpeed> CarSpeeds { get; set; }
        public DbSet<CarSpeedRead> CarSpeedsRead { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyRead> CompaniesRead { get; set; }
    }
}