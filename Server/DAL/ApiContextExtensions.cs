using System;
using System.Linq;
using Shared.Models;

namespace Server.DAL
{
    public static class ApiContextExtensions
    {
        public static void EnsureSeedData(this ApiContext context)
        {
            if (!context.Cars.Any() || !context.Companies.Any())
            {
                var companyId = Guid.NewGuid();
                context.Companies.Add(new Company() { CompanyId = companyId, Name = "Charlies Gravel Transports Ltd.", Address = "Concrete Road 8, 111 11 Newcastle" });
                var carId = Guid.NewGuid();
                var car = new Car
                {
                    CarId = carId,
                    CompanyId = companyId,
                    VIN = "YS2R4X20005399401",
                    RegNr = "ABC123"
                };
                context.Cars.Add(car);

                carId = Guid.NewGuid();
                car = new Car
                {
                    CarId = carId,
                    CompanyId = companyId,
                    VIN = "VLUR4X20009093588",
                    RegNr = "DEF456"
                };
                context.Cars.Add(car);

                carId = Guid.NewGuid();
                car = new Car
                {
                    CarId = carId,
                    CompanyId = companyId,
                    VIN = "VLUR4X20009048066",
                    RegNr = "GHI789"
                };
                context.Cars.Add(car);

                companyId = Guid.NewGuid();
                context.Companies.Add(new Company() { CompanyId = companyId, Name = "Jonnies Bulk Ltd.", Address = "Balk Road 12, 222 22 London" });

                carId = Guid.NewGuid();
                car = new Car
                {
                    CarId = carId,
                    CompanyId = companyId,
                    VIN = "YS2R4X20005388011",
                    RegNr = "JKL012"
                };
                context.Cars.Add(car);

                carId = Guid.NewGuid();
                car = new Car
                {
                    CarId = carId,
                    CompanyId = companyId,
                    VIN = "YS2R4X20005387949",
                    RegNr = "MNO345"
                };
                context.Cars.Add(car);

                companyId = Guid.NewGuid();
                context.Companies.Add(new Company() { CompanyId = companyId, Name = "Harolds Road Transports Ltd.", Address = "Budget Avenue 1, 333 33 Birmingham" });

                carId = Guid.NewGuid();
                car = new Car
                {
                    CarId = carId,
                    CompanyId = companyId,
                    VIN = "YS2R4X20005387765",
                    RegNr = "PQR678"
                };
                context.Cars.Add(car);

                carId = Guid.NewGuid();
                car = new Car
                {
                    CarId = carId,
                    CompanyId = companyId,
                    VIN = "YS2R4X20005387055",
                    RegNr = "STU901"
                };
                context.Cars.Add(car);
            }
            else
            {
                foreach (var car in context.Cars)
                {
                    car.Locked = false;
                }
            }
            context.SaveChanges();
        }
    }
}
