using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Server.DAL
{
	public class ApiContextNew : DbContext
	{
		public ApiContextNew(DbContextOptions<ApiContextNew> options)
			: base(options)
		{
		}

		public DbSet<Car> Cars { get; set; }
		public DbSet<CarOnlineStatus> CarOnlineStatus { get; set; }
		public DbSet<CarCompany> CarCompany { get; set; }
		public DbSet<CarDisabledStatus> CarDisabledStatus { get; set; }
		public DbSet<Company> Companies { get; set; }
	}
}