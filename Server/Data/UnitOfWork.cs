using Server.DAL;

namespace Server.Data
{
	public class UnitOfWork:IUnitOfWork
    {
	    readonly ApiContext _context;

	    public UnitOfWork(ApiContext context)
	    {
		    _context = context;
		    Cars = new CarRepository(_context);
		    Companies = new CompanyRepository(_context);
		}

	    public void Dispose()
	    {
		   Context.Dispose();
	    }

	    public ICarRepository Cars { get; private set; }
	    public ICompanyRepository Companies { get; private set; }

		public ApiContext Context => Context1;

		public ApiContext Context1 => _context;

		public int Complete()
		{
			return Context.SaveChanges();
		}
    }
}
