using Server.DAL;

namespace Server.Data
{
	public class CarUnitOfWork:ICarUnitOfWork
    {
	    readonly ApiContext _context;

	    public CarUnitOfWork(ApiContext context)
	    {
		    _context = context;
		    Cars = new CarRepository(_context);
            CarLockedStatuses = new CarLockedStatusRepository(_context);
            CarOnlineStatuses = new CarOnlineStatusRepository(_context);
            CarSpeeds = new CarSpeedRepository(_context);
            Companies = new CompanyRepository(_context);
		}

	    public void Dispose()
	    {
		   _context.Dispose();
	    }

	    public ICarRepository Cars { get; private set; }
        public ICarLockedStatusRepository CarLockedStatuses { get; private set; }
        public ICarOnlineStatusRepository CarOnlineStatuses { get; private set; }
        public ICarSpeedRepository CarSpeeds { get; private set; }
        public ICompanyRepository Companies { get; private set; }
		public int Complete()
		{
			return _context.SaveChanges();
		}
    }
}
