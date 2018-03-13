using System;
using System.Linq;
using Shared.Models.Write;
using Shared.Models.Read;

namespace Server.DAL
{
    public static class ApiContextExtensions
    {
        public static void EnsureSeedData(this ApiContext context)
        {
            if (!context.Cars.Any() || !context.Companies.Any())
            {
                var companyId = Guid.NewGuid();
                var company = new Company(companyId) { Name = "Charlies Gravel Transports Ltd.", Address = "Concrete Road 8, 111 11 Newcastle" };
                context.Companies.Add(company);
                MapCompanyToCompanyReadNulls(context, company);
                CreateAddress(context, company);
                CreateName(context, company);
                var carId = Guid.NewGuid();
                var car = new Car(carId)
                {
                    CompanyId = companyId,
                    VIN = "YS2R4X20005399401",
                    RegNr = "ABC123"
                };
                context.Cars.Add(car);
                MapCarToCarReadNulls(context, car);
                CreateLockedStatus(context, car);
                CreateOnlineStatus(context, car);
                CreateSpeed(context, car);

                carId = Guid.NewGuid();
                car = new Car(carId)
                {
                    CompanyId = companyId,
                    VIN = "VLUR4X20009093588",
                    RegNr = "DEF456"
                };
                context.Cars.Add(car);
                MapCarToCarReadNulls(context, car);
                CreateLockedStatus(context, car);
                CreateOnlineStatus(context, car);
                CreateSpeed(context, car);

                carId = Guid.NewGuid();
                car = new Car(carId)
                {
                    CompanyId = companyId,
                    VIN = "VLUR4X20009048066",
                    RegNr = "GHI789"
                };
                context.Cars.Add(car);
                MapCarToCarReadNulls(context, car);
                CreateLockedStatus(context, car);
                CreateOnlineStatus(context, car);
                CreateSpeed(context, car);

                companyId = Guid.NewGuid();
                company = new Company(companyId){ Name = "Jonnies Bulk Ltd.", Address = "Balk Road 12, 222 22 London" };
                context.Companies.Add(company);
                MapCompanyToCompanyReadNulls(context, company);
                CreateAddress(context, company);
                CreateName(context, company);

                carId = Guid.NewGuid();
                car = new Car(carId)
                {
                    CompanyId = companyId,
                    VIN = "YS2R4X20005388011",
                    RegNr = "JKL012"
                };
                context.Cars.Add(car);
                MapCarToCarReadNulls(context, car);
                CreateLockedStatus(context, car);
                CreateOnlineStatus(context, car);
                CreateSpeed(context, car);

                carId = Guid.NewGuid();
                car = new Car(carId)
                {
                    CompanyId = companyId,
                    VIN = "YS2R4X20005387949",
                    RegNr = "MNO345"
                };
                context.Cars.Add(car);
                MapCarToCarReadNulls(context, car);
                CreateLockedStatus(context, car);
                CreateOnlineStatus(context, car);
                CreateSpeed(context, car);

                companyId = Guid.NewGuid();
                company = new Company(companyId) { Name = "Harolds Road Transports Ltd.", Address = "Budget Avenue 1, 333 33 Birmingham" };
                context.Companies.Add(company);
                MapCompanyToCompanyReadNulls(context, company);
                CreateAddress(context, company);
                CreateName(context, company);

                carId = Guid.NewGuid();
                car = new Car(carId)
                {
                    CompanyId = companyId,
                    VIN = "YS2R4X20005387765",
                    RegNr = "PQR678"
                };
                context.Cars.Add(car);
                MapCarToCarReadNulls(context, car);
                CreateLockedStatus(context, car);
                CreateOnlineStatus(context, car);
                CreateSpeed(context, car);

                carId = Guid.NewGuid();
                car = new Car(carId)
                {
                    CompanyId = companyId,
                    VIN = "YS2R4X20005387055",
                    RegNr = "STU901"
                };
                context.Cars.Add(car);
                MapCarToCarReadNulls(context, car);
                CreateLockedStatus(context, car);
                CreateOnlineStatus(context, car);
                CreateSpeed(context, car);

                context.SaveChanges();
            }
        }

        private static void CreateLockedStatus(ApiContext context, Car car)
        {
            context.CarLockedStatuses.Add(new CarLockedStatus { Locked = false, CarId = car.CarId, LockedTimeStamp = 0 });
        }

        private static void CreateLockedStatusRead(ApiContext context, Car car)
        {
            context.CarLockedStatusesRead.Add(new CarLockedStatusRead { Locked = false, CarId = car.CarId, LockedTimeStamp = 0 });
        }
        private static void CreateOnlineStatus(ApiContext context, Car car)
        {
            context.CarOnlineStatuses.Add(new CarOnlineStatus { Online = false, CarId = car.CarId, OnlineTimeStamp = 0 });
        }

        private static void CreateOnlineStatusRead(ApiContext context, Car car)
        {
            context.CarOnlineStatusesRead.Add(new CarOnlineStatusRead { Online = false, CarId = car.CarId, OnlineTimeStamp = 0 });
        }

        private static void CreateSpeed(ApiContext context, Car car)
        {
            context.CarSpeeds.Add(new CarSpeed { Speed = 567, CarId = car.CarId, SpeedTimeStamp = 0 });
        }

        private static void CreateSpeedRead(ApiContext context, Car car)
        {
            context.CarSpeedsRead.Add(new CarSpeedRead { Speed = 567, CarId = car.CarId, SpeedTimeStamp = 0 });
        }

        private static void MapCarToCarReadNulls(ApiContext context, Car car)
        {
            context.CarReadNulls.Add(new CarReadNull(car.CarId,car.CompanyId)
            {
                CompanyId = car.CompanyId,
                CreationTime = car.CreationTime,
                RegNr = car.RegNr,
                VIN = car.VIN,
                Locked = false,
                Online = true,
                Speed = 567
            });
        }

        private static void MapCompanyToCompanyReadNulls(ApiContext context, Company company)
        {
            context.CompanyReadNulls.Add(new CompanyReadNull(company.CompanyId)
            {
                CompanyId = company.CompanyId,
                CreationTime = company.CreationTime,
                Address = company.Address,
                Name = company.Name
            });
        }

        private static void CreateAddress(ApiContext context, Company company)
        {
            context.CompanyAddresses.Add(new CompanyAddress { Address = company.Address, CompanyId = company.CompanyId,  AddressTimeStamp = 0 });
        }

        private static void CreateName(ApiContext context, Company company)
        {
            context.CompanyNames.Add(new CompanyName { Name = company.Name, CompanyId = company.CompanyId, NameTimeStamp = 0 });
        }
    }
}