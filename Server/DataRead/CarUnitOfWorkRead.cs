using Shared.Models.Read;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.DataRead
{
	public class CarUnitOfWorkRead:ICarUnitOfWorkRead
    {
	    readonly ApiContext _context;

	    public CarUnitOfWorkRead(ApiContext context)
	    {
		    _context = context;
		    Cars = new CarRepositoryRead(_context);
            CarOnlineStatus = new CarOnlineStatusRepositoryRead(_context);
            CarLockedStatus = new CarLockedStatusRepositoryRead(_context);
            CarSpeed = new CarSpeedRepositoryRead(_context);
            Companies = new CompanyRepositoryRead(_context);
		}

	    public void Dispose()
	    {
		   _context.Dispose();
	    }

	    public ICarRepositoryRead Cars { get; private set; }
        public ICarOnlineStatusRepositoryRead CarOnlineStatus { get; private set; }
        public ICarLockedStatusRepositoryRead CarLockedStatus { get; private set; }
        public ICarSpeedRepositoryRead CarSpeed { get; private set; }
        public ICompanyRepositoryRead Companies { get; private set; }
		public int Complete()
		{
			return _context.SaveChanges();
		}
    }
}
