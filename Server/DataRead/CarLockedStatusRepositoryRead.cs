using Shared.Models.Read;
using Server.DAL;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Server.DataRead
{
	public class CarLockedStatusRepositoryRead : RepositoryRead<CarLockedStatusRead>, ICarLockedStatusRepositoryRead
	{
		public CarLockedStatusRepositoryRead(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CarLockedStatusRead> GetAllOrdered(Guid CarId)
        {
            return GetAll().Where(o => o.CarId == CarId).OrderBy(o => o.LockedTimeStamp).ToList();
        }
    }
}
