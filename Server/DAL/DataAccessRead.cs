using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Read;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Server.DAL
{
    public class DataAccessRead
    {
	    public DataAccessRead(IConfigurationRoot configuration)
	    {
		    Configuration = configuration;
            var serverFolder = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + Path.DirectorySeparatorChar + "Server" + Path.DirectorySeparatorChar;
            _optionsBuilder.UseSqlite("DataSource=" + serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "Car.db");
        }
	    IConfigurationRoot Configuration { get; set; }

		private readonly DbContextOptionsBuilder<ApiContext> _optionsBuilder = new DbContextOptionsBuilder<ApiContext>();


	    public ICollection<CarRead> GetCars()
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
			    return context.CarsRead.ToList();
		    }
	    }

        public CarRead GetCar(Guid carId)
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
			    return context.CarsRead.FirstOrDefault(o => o.CarId == carId);
		    }
	    }

		public ICollection<CompanyRead> GetCompanies()
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                return context.CompaniesRead.ToList();
            }
        }

        public CompanyRead GetCompany(Guid companyId)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                return context.CompaniesRead.FirstOrDefault(o => o.CompanyId == companyId);
            }
        }
    }
}