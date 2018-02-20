using Shared.Models;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.Data
{
	public class CarSpeedRepository : Repository<CarSpeed>, ICarSpeedRepository
	{
		public CarSpeedRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CarSpeed> GetAllOrdered(Guid CarId)
        {
            return GetAll().Where(o => o.CarId == CarId).OrderBy(o => o.SpeedTimeStamp).ToList();
        }

    }
}
