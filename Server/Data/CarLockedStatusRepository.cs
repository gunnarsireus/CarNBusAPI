using Shared.Models;
using Server.DAL;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Server.Data
{
	public class CarLockedStatusRepository : Repository<CarLockedStatus>, ICarLockedStatusRepository
	{
		public CarLockedStatusRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CarLockedStatus> GetAllOrdered(Guid CarId)
        {
            return GetAll().Where(o => o.CarId == CarId).OrderBy(o => o.LockedTimeStamp).ToList();
        }
    }
}
