using Microsoft.EntityFrameworkCore;
using CarNBusAPI.Models;

namespace CarNBusAPI.DAL
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions options)
            : base(options)
        {
        }

	    public DbSet<Car> Cars { get; set; }

	    public DbSet<Company> Companies { get; set; }
	}
}