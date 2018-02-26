using Shared.Models.Write;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.Data
{
	public class CompanyNameRepository : Repository<CompanyName>, ICompanyNameRepository
    {
		public CompanyNameRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;
    }
}
