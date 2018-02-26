using Shared.Models.Read;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.DataRead
{
	public class CompanyAddressRepositoryRead : RepositoryRead<CompanyAddressRead>, ICompanyAddressRepositoryRead
	{
		public CompanyAddressRepositoryRead(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CompanyAddressRead> GetAllOrdered(Guid CompanyId)
        {
            return GetAll().Where(o => o.CompanyId == CompanyId).OrderBy(o => o.AddressTimeStamp).ToList();
        }
    }
}
