using Shared.Models;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.Data
{
	public class CarOnlineStatusRepository : Repository<CarOnlineStatus>, ICarOnlineStatusRepository
	{
		public CarOnlineStatusRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CarOnlineStatus> GetAllOrdered(Guid CarId)
        {
            return GetAll().Where(o => o.CarId == CarId).OrderBy(o => o.OnlineTimeStamp).ToList();
        }
    }
}
