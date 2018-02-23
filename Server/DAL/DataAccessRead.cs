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
            var carReads = new List<CarRead>();
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                var uniqueCarReadNull = context.CarsReadNull.GroupBy(i => i.CarId).Select(g => g.First()).ToList();
                foreach (var carReadNull in uniqueCarReadNull)
                {
                    var onlineList = context.CarsReadNull.Where(w => (w.Online != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => s.Online ?? false).ToList();
                    var lockedList = context.CarsReadNull.Where(w => (w.Locked != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Locked = s.Locked ?? false, s.LockedTimeStamp }).ToList();
                    var speedList = context.CarsReadNull.Where(w => (w.Speed != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => s.Speed ?? 0).ToList();
                    var deletedList = context.CarsReadNull.Where(w => w.CarId == carReadNull.CarId).OrderBy(c => c.ChangeTimeStamp).Select(s => s.Deleted).ToList();
                    if (!deletedList.LastOrDefault())
                    {
                        carReads.Add(new CarRead(carReadNull.CarId)
                        {
                            ChangeTimeStamp = carReadNull.ChangeTimeStamp,
                            LockedTimeStamp = lockedList.LastOrDefault().LockedTimeStamp,
                            CompanyId = carReadNull.CompanyId,
                            CreationTime = carReadNull.CreationTime,
                            RegNr = carReadNull.RegNr,
                            VIN = carReadNull.VIN,
                            Speed = speedList.LastOrDefault(),
                            Online = onlineList.LastOrDefault(),
                            Locked = lockedList.LastOrDefault().Locked
                        });
                    }
                }

                return carReads;
            }
        }

        public CarRead GetCar(Guid carId)
        {

            CarRead carRead = null;
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                var carReadNull = context.CarsReadNull.OrderBy(c => c.ChangeTimeStamp).FirstOrDefault(o => o.CarId == carId && !o.Deleted);
                if (carReadNull != null)
                {
                    var onlineList = context.CarsReadNull.Where(w => (w.Online != null && w.CarId == carId)).OrderBy(c => c.ChangeTimeStamp).Select(s => s.Online ?? false).ToList();
                    var lockedList = context.CarsReadNull.Where(w => (w.Locked != null && w.CarId == carId)).OrderBy(c => c.LockedTimeStamp).Select(s => new { Locked = s.Locked ?? false, s.LockedTimeStamp }).ToList();
                    var speedList = context.CarsReadNull.Where(w => (w.Speed != null && w.CarId == carId)).OrderBy(c => c.ChangeTimeStamp).Select(s => s.Speed ?? 0).ToList();
                    var deletedList = context.CarsReadNull.Where(w => w.CarId == carReadNull.CarId).OrderBy(c => c.ChangeTimeStamp).Select(s => s.Deleted).ToList();
                    if (!deletedList.LastOrDefault())
                    {
                        carRead = new CarRead(carId)
                        {
                            ChangeTimeStamp = carReadNull.ChangeTimeStamp,
                            LockedTimeStamp = lockedList.LastOrDefault().LockedTimeStamp,
                            CompanyId = carReadNull.CompanyId,
                            CreationTime = carReadNull.CreationTime,
                            RegNr = carReadNull.RegNr,
                            VIN = carReadNull.VIN,
                            Speed = speedList.LastOrDefault(),
                            Online = onlineList.LastOrDefault(),
                            Locked = lockedList.LastOrDefault().Locked
                        };
                    }
                }

                return carRead;
            }
        }

        public ICollection<CompanyRead> GetCompanies()
        {
            var CompanyReads = new List<CompanyRead>();
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                var uniqueCompaniesRead = context.CompaniesRead.GroupBy(i => i.CompanyId).Select(g => g.First()).ToList();
                foreach (var companyRead in uniqueCompaniesRead)
                {
                    var deletedList = context.CompaniesRead.Where(w => w.CompanyId == companyRead.CompanyId).OrderBy(c => c.ChangeTimeStamp).Select(s => s.Deleted).ToList();
                    if (!deletedList.LastOrDefault())
                    {
                        CompanyReads.Add(new CompanyRead(companyRead.CompanyId)
                        {
                            ChangeTimeStamp = companyRead.ChangeTimeStamp,
                            CompanyId = companyRead.CompanyId,
                            CreationTime = companyRead.CreationTime,
                            Address = companyRead.Address,
                            Name = companyRead.Name,
                            Pending = companyRead.Pending,
                            Deleted = companyRead.Deleted
                        });
                    }
                }

                return CompanyReads;
            }
         }

        public CompanyRead GetCompany(Guid companyId)
        {
            CompanyRead companyRead = null;
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                companyRead = context.CompaniesRead.OrderBy(c => c.ChangeTimeStamp).LastOrDefault(o => o.CompanyId == companyId && !o.Deleted);
                if (companyRead != null)
                {
                    var deletedList = context.CompaniesRead.Where(w => w.CompanyId == companyId).OrderBy(c => c.ChangeTimeStamp).Select(s => s.Deleted).ToList();
                    if (!deletedList.LastOrDefault())
                    {
                        return companyRead;
                    }
                }

                return null;
            }
        }
    }
}