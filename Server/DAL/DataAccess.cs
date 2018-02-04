using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Microsoft.Extensions.Configuration;
using Autofac;
using System.IO;

namespace Server.DAL
{
    public class DataAccess
    {
	    public DataAccess(IConfigurationRoot configuration)
	    {
		    Configuration = configuration;
            var serverFolder = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + Path.DirectorySeparatorChar + "Server" + Path.DirectorySeparatorChar;
            _optionsBuilder.UseSqlite("DataSource=" + serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "Car.db");
        }
	    IConfigurationRoot Configuration { get; set; }

		private readonly DbContextOptionsBuilder<ApiContext> _optionsBuilder = new DbContextOptionsBuilder<ApiContext>();


	    public ICollection<Car> GetCars()
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
			    return context.Cars.ToList();
		    }
	    }

        public CarOnlineStatus GetCarOnlineStatus(Guid carId)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                return context.CarOnlineStatus.FirstOrDefault(c=>c.CarId==carId);
            }
        }

        public CarLockedStatus GetCarLockedStatus(Guid carId)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                return context.CarLockedStatus.FirstOrDefault(c => c.CarId == carId);
            }
        }

        public Car GetCar(Guid carId)
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
			    return context.Cars.SingleOrDefault(o => o.CarId == carId);
		    }
	    }

	    public void AddCar(Car car)
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
			    context.Cars.Add(car);
			    context.SaveChanges();
		    }
	    }

	    public void DeleteCar(Guid carId)
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
			    var Car = GetCar(carId);
			    context.Cars.Remove(Car);
			    context.SaveChanges();
		    }
	    }

	    public void UpdateCar(Car car)
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
			    context.Cars.Update(car);
			    context.SaveChanges();
		    }
	    }

		public ICollection<Company> GetCompanies()
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                return context.Companies.ToList();
            }
        }

        public Company GetCompany(Guid companyId)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                return context.Companies.SingleOrDefault(o => o.CompanyId == companyId);
            }
        }

        public void AddCompany(Company company)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.Companies.Add(company);
                context.SaveChanges();
            }
        }

        public void DeleteCompany(Guid companyId)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                var cars = GetCars().Where(c => c.CompanyId == companyId);
                foreach (var car in cars)
                {
                    context.Cars.Remove(car);
                }

                var company = GetCompany(companyId);
                context.Companies.Remove(company);
                context.SaveChanges();
            }
        }

        public void UpdateCompany(Company company)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.Companies.Update(company);
                context.SaveChanges();
            }
        }
    }
}