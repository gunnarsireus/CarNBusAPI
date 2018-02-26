using Server.DAL;

namespace Server.DataRead
{
    public class UnitOfWorkRead : IUnitOfWork
    {
        readonly ApiContext _context;

        public UnitOfWorkRead(ApiContext context)
        {
            _context = context;
            Cars = new CarRepositoryRead(_context);
            CarOnlineStatus = new CarOnlineStatusRepositoryRead(_context);
            CarLockedStatus = new CarLockedStatusRepositoryRead(_context);
            CarSpeeds = new CarSpeedRepositoryRead(_context);
            Companies = new CompanyRepositoryRead(_context);
            CompanyNames = new CompanyNameRepositoryRead(_context);
            CompanyAddresses = new CompanyAddressRepositoryRead(_context);
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public ICarRepositoryRead Cars { get; private set; }
        public ICarOnlineStatusRepositoryRead CarOnlineStatus { get; private set; }
        public ICarLockedStatusRepositoryRead CarLockedStatus { get; private set; }
        public ICarSpeedRepositoryRead CarSpeeds { get; private set; }
        public ICompanyRepositoryRead Companies { get; private set; }
        public ICompanyNameRepositoryRead CompanyNames { get; private set; }
        public ICompanyAddressRepositoryRead CompanyAddresses { get; private set; }

        public ApiContext Context => _context;
        public int Complete()
        {
            return 0;
        }
    }
}
