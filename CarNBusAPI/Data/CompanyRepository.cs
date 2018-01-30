using CarNBusAPI.DAL;
using CarNBusAPI.Models;


namespace CarNBusAPI.Data
{
	public class CompanyRepository : Repository<Company>, ICompanyRepository
	{
		public CompanyRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

	}
}
