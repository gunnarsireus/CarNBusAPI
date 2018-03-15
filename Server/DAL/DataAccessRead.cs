using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Read;
using Microsoft.Extensions.Configuration;
using System.IO;
using Shared.Utils;

namespace Server.DAL
{
    public class DataAccessRead
    {
        public DataAccessRead(IConfigurationRoot configuration)
        {
            ConfigurationRoot = configuration;
            _optionsBuilder.UseSqlite("DataSource=" + Helpers.GetDbLocation(ConfigurationRoot["AppSettings:DbLocation"]) + "Car.db");
        }
        IConfigurationRoot ConfigurationRoot { get; set; }

        private readonly DbContextOptionsBuilder<ApiContext> _optionsBuilder = new DbContextOptionsBuilder<ApiContext>();


        public ICollection<CarRead> GetCars()
        {
            var carReads = new List<CarRead>();
            using (var context = new ApiContext(_optionsBuilder.Options))
            {
                var uniqueCarReadNull = context.CarReadNulls.OrderBy(c => c.ChangeTimeStamp).GroupBy(i => i.CarId).Select(g => g.First()).ToList();
                foreach (var carReadNull in uniqueCarReadNull)
                {
                    var onlineList = context.CarReadNulls.Where(w => (w.Online != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Online = s.Online ?? false, s.ChangeTimeStamp }).ToList();
                    var lockedList = context.CarReadNulls.Where(w => (w.Locked != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Locked = s.Locked ?? false, LockedTimeStamp = s.ChangeTimeStamp }).ToList();
                    var speedList = context.CarReadNulls.Where(w => (w.Speed != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Speed = s.Speed ?? 0, s.ChangeTimeStamp }).ToList();
                    var deletedList = context.CarReadNulls.Where(w => (w.Deleted != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Deleted = s.Deleted ?? false, s.ChangeTimeStamp }).ToList();
                    bool IsDeleted = (!deletedList.Any()) ? false : deletedList.LastOrDefault().Deleted;
                    if (!IsDeleted)
                    {
                        carReads.Add(new CarRead(carReadNull.CarId)
                        {
                            CompanyId = carReadNull.CompanyId,
                            CreationTime = carReadNull.CreationTime,
                            RegNr = carReadNull.RegNr,
                            VIN = carReadNull.VIN,
                            Speed = speedList.LastOrDefault().Speed,
                            Online = onlineList.LastOrDefault().Online,
                            Locked = lockedList.LastOrDefault().Locked,
                            LockedTimeStamp = lockedList.LastOrDefault().LockedTimeStamp
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
                var carReadNull = context.CarReadNulls.OrderBy(c => c.ChangeTimeStamp).FirstOrDefault(o=>o.CarId==carId);
                if (carReadNull != null)
                {
                    var onlineList = context.CarReadNulls.Where(w => (w.Online != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Online = s.Online ?? false, s.ChangeTimeStamp }).ToList();
                    var lockedList = context.CarReadNulls.Where(w => (w.Locked != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Locked = s.Locked ?? false, LockedTimeStamp = s.ChangeTimeStamp }).ToList();
                    var speedList = context.CarReadNulls.Where(w => (w.Speed != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Speed = s.Speed ?? 0, s.ChangeTimeStamp }).ToList();
                    var deletedList = context.CarReadNulls.Where(w => (w.Deleted != null && w.CarId == carReadNull.CarId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Deleted = s.Deleted ?? false, s.ChangeTimeStamp }).ToList();
                    bool IsDeleted = (!deletedList.Any()) ? false : deletedList.LastOrDefault().Deleted;
                    if (!IsDeleted)
                    {
                        carRead = new CarRead(carId)
                        {
                            CompanyId = carReadNull.CompanyId,
                            CreationTime = carReadNull.CreationTime,
                            RegNr = carReadNull.RegNr,
                            VIN = carReadNull.VIN,
                            Speed = speedList.LastOrDefault().Speed,
                            Online = onlineList.LastOrDefault().Online,
                            Locked = lockedList.LastOrDefault().Locked,
                            LockedTimeStamp = lockedList.LastOrDefault().LockedTimeStamp
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
                var uniqueCompanyReadNull = context.CompanyReadNulls.OrderBy(c => c.ChangeTimeStamp).GroupBy(i => i.CompanyId).Select(g => g.First()).ToList();
                foreach (var companyReadNull in uniqueCompanyReadNull)
                {
                    var addressList = context.CompanyReadNulls.Where(w => (w.Address != null && w.CompanyId == companyReadNull.CompanyId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Address = s.Address ?? "", s.ChangeTimeStamp }).ToList();
                    var nameList = context.CompanyReadNulls.Where(w => (w.Name != null && w.CompanyId == companyReadNull.CompanyId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Name = s.Name ?? "", s.ChangeTimeStamp }).ToList();
                    var deletedList = context.CompanyReadNulls.Where(w => (w.Deleted != null && w.CompanyId == companyReadNull.CompanyId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Deleted = s.Deleted ?? false, s.ChangeTimeStamp }).ToList();
                    bool IsDeleted = (!deletedList.Any()) ? false : deletedList.LastOrDefault().Deleted;
                    if (!IsDeleted)
                    {
                        CompanyReads.Add(new CompanyRead(companyReadNull.CompanyId)
                        {
                            CompanyId = companyReadNull.CompanyId,
                            CreationTime = companyReadNull.CreationTime,
                            Name = nameList.LastOrDefault().Name,
                            Address = addressList.LastOrDefault().Address
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
                var companyReadNull = context.CompanyReadNulls.OrderBy(c => c.ChangeTimeStamp).FirstOrDefault(o => o.CompanyId == companyId);
                if (companyReadNull != null)
                {
                    var addressList = context.CompanyReadNulls.Where(w => (w.Address != null && w.CompanyId == companyReadNull.CompanyId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Address = s.Address ?? "", s.ChangeTimeStamp }).ToList();
                    var nameList = context.CompanyReadNulls.Where(w => (w.Name != null && w.CompanyId == companyReadNull.CompanyId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Name = s.Name ?? "", s.ChangeTimeStamp }).ToList();
                    var deletedList = context.CompanyReadNulls.Where(w => (w.Deleted != null && w.CompanyId == companyReadNull.CompanyId)).OrderBy(c => c.ChangeTimeStamp).Select(s => new { Deleted = s.Deleted ?? false, s.ChangeTimeStamp }).ToList();
                    bool IsDeleted = (!deletedList.Any()) ? false : deletedList.LastOrDefault().Deleted;
                    if (!IsDeleted)
                    {
                        companyRead = new CompanyRead(companyId)
                        {
                            CompanyId = companyReadNull.CompanyId,
                            CreationTime = companyReadNull.CreationTime,
                            Name = nameList.LastOrDefault().Name,
                            Address = addressList.LastOrDefault().Address
                        };
                    }
                }
                return companyRead;
            }
        }
    }
}