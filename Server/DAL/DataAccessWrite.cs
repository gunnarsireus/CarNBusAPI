using System;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Write;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Server.DAL
{
    public class DataAccessWrite
    {
	    public DataAccessWrite(IConfigurationRoot configuration)
	    {
		    Configuration = configuration;
            var serverFolder = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + Path.DirectorySeparatorChar + "Server" + Path.DirectorySeparatorChar;
            _optionsBuilder.UseSqlite("DataSource=" + serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "Car.db");
        }
	    IConfigurationRoot Configuration { get; set; }

		private readonly DbContextOptionsBuilder<ApiContext> _optionsBuilder = new DbContextOptionsBuilder<ApiContext>();

	    public void AddCar(Car car)
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
			    context.Cars.Add(car);
			    context.SaveChanges();
		    }
	    }

        public void AddLockedStatus(CarLockedStatus carLockedStatus)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarLockedStatuses.Add(carLockedStatus);
                context.SaveChanges();
            }
        }

        public void AddOnlineStatus(CarOnlineStatus carOnlineStatus)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarOnlineStatuses.Add(carOnlineStatus);
                context.SaveChanges();
            }
        }

        public void AddCarSpeed(CarSpeed carSpeed)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarSpeeds.Add(carSpeed);
                context.SaveChanges();
            }
        }

        public void DeleteLockedStatus(CarLockedStatus carLockedStatus)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarLockedStatuses.Remove(carLockedStatus);
                context.SaveChanges();
            }
        }

        public void DeleteOnlineStatus(CarOnlineStatus carOnlineStatus)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarOnlineStatuses.Remove(carOnlineStatus);
                context.SaveChanges();
            }
        }

        public void DeleteCarSpeed(CarSpeed carSpeed)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarSpeeds.Remove(carSpeed);
                context.SaveChanges();
            }
        }

        public void UpdateLockedStatus(CarLockedStatus carLockedStatus)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarLockedStatuses.Update(carLockedStatus);
                context.SaveChanges();
            }
        }

        public void UpdateOnlineStatus(CarOnlineStatus carOnlineStatus)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarOnlineStatuses.Update(carOnlineStatus);
                context.SaveChanges();
            }
        }

        public void UpdateCarSpeed(CarSpeed carSpeed)
        {
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                context.CarSpeeds.Update(carSpeed);
                context.SaveChanges();
            }
        }

        public void DeleteCar(Guid carId)
	    {
		    using (var context = new ApiContext(_optionsBuilder.Options))
		    {
                var car = new Car(carId);//GetCar(carId);
			    context.Cars.Remove(car);
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
                var company = new Company(companyId); //GetCompany(companyId);
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