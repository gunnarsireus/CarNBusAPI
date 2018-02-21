using Shared.Models.Read;
using Server.DAL;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Server.DataRead
{
	public class CarRepositoryRead : RepositoryRead<CarRead>, ICarRepositoryRead
	{
		public CarRepositoryRead(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CarRead> GetAllByCompanyId(Guid CompanyId)
        {
            return GetAll().Where(o => o.CompanyId == CompanyId).ToList();
        }
    }
}
