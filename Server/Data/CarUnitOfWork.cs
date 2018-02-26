﻿using Server.DAL;
using Server.DataRead;

namespace Server.Data
{
	public class CarUnitOfWork:ICarUnitOfWork
    {
	    readonly ApiContext _context;

	    public CarUnitOfWork(ApiContext context)
	    {
		    _context = context;
		    Cars = new CarRepository(_context);
            CarsReadNull = new CarReadRepository(_context);
            CompanyReadNulls = new CompanyReadRepository(_context);
            CarLockedStatuses = new CarLockedStatusRepository(_context);
            CarOnlineStatuses = new CarOnlineStatusRepository(_context);
            CarSpeeds = new CarSpeedRepository(_context);
            Companies = new CompanyRepository(_context);
            CompanyNames = new CompanyNameRepository(_context);
            CompanyAddresses = new CompanyAddressRepository(_context);
        }

	    public void Dispose()
	    {
		   _context.Dispose();
	    }

	    public ICarRepository Cars { get; private set; }
        public ICarReadRepository CarsReadNull { get; private set; }
        public ICarLockedStatusRepository CarLockedStatuses { get; private set; }
        public ICarOnlineStatusRepository CarOnlineStatuses { get; private set; }
        public ICarSpeedRepository CarSpeeds { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public ICompanyReadRepository CompanyReadNulls { get; private set; }
        public ICompanyNameRepository CompanyNames { get; private set; }
        public ICompanyAddressRepository CompanyAddresses { get; private set; }

        public int Complete()
		{
			return _context.SaveChanges();
		}
    }
}
