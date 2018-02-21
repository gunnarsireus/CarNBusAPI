using Shared.Models.Read;
using Server.DAL;
using Microsoft.EntityFrameworkCore;

namespace Server.DataRead
{
    public class CompanyRepositoryRead : RepositoryRead<CompanyRead>, ICompanyRepositoryRead
	{
		public CompanyRepositoryRead(DbContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;
	}
}
