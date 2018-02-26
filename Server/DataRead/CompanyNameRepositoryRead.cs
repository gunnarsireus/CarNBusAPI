using Shared.Models.Read;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.DataRead
{
	public class CompanyNameRepositoryRead : RepositoryRead<CompanyNameRead>, ICompanyNameRepositoryRead
	{
		public CompanyNameRepositoryRead(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CompanyNameRead> GetAllOrdered(Guid CompanyId)
        {
            return GetAll().Where(o => o.CompanyId == CompanyId).OrderBy(o => o.NameTimeStamp).ToList();
        }
    }
}
