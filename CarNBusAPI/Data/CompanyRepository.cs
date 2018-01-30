using CarNBusAPI.DAL;
using CarNBusAPI.Models;


namespace CarNBusAPI.Data
{
	public class CompanyRepository : Repository<Company>, ICompanyRepository
	{
		public CompanyRepository(CarNBusAPIContext context) : base(context)
		{
		}

		public CarNBusAPIContext CarNBusAPIContext => Context as CarNBusAPIContext;

	}
}
