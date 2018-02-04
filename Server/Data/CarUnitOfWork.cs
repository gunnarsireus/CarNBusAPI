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
            CarOnlineStatus = new CarOnlineStatusRepository(_context);
            CarLockedStatus = new CarLockedStatusRepository(_context);
            Companies = new CompanyRepository(_context);
		}

	    public void Dispose()
	    {
		   _context.Dispose();
	    }

	    public ICarRepository Cars { get; private set; }
        public ICarOnlineStatusRepository CarOnlineStatus { get; private set; }
        public ICarLockedStatusRepository CarLockedStatus { get; private set; }
        public ICompanyRepository Companies { get; private set; }
		public int Complete()
		{
			return _context.SaveChanges();
		}
    }
}
