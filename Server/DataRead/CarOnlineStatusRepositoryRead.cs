using Shared.Models.Read;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.DataRead
{
	public class CarOnlineStatusRepositoryRead : RepositoryRead<CarOnlineStatusRead>, ICarOnlineStatusRepositoryRead
	{
		public CarOnlineStatusRepositoryRead(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CarOnlineStatusRead> GetAllOrdered(Guid CarId)
        {
            return GetAll().Where(o => o.CarId == CarId).OrderBy(o => o.OnlineTimeStamp).ToList();
        }
    }
}
