using CarNBusAPI.DAL;

namespace CarNBusAPI.Data
{
    public class CarUnitOfWork:ICarUnitOfWork
    {
	    private readonly CarNBusAPIContext _context;

	    public CarUnitOfWork(CarNBusAPIContext context)
	    {
		    _context = context;
		    Cars = new CarRepository(_context);
		    Companies = new CompanyRepository(_context);
		}

	    public void Dispose()
	    {
		   _context.Dispose();
	    }

	    public ICarRepository Cars { get; private set; }
	    public ICompanyRepository Companies { get; private set; }
		public int Complete()
		{
			return _context.SaveChanges();
		}
    }
}
