using Shared.Models.Read;
using Server.DAL;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Server.Data
{
	public class CompanyReadRepository : Repository<CompanyReadNull>, ICompanyReadRepository
	{
		public CompanyReadRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;
    }
}
