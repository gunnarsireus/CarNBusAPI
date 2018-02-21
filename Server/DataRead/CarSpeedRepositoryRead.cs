using Shared.Models.Read;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.DataRead
{
	public class CarSpeedRepositoryRead : RepositoryRead<CarSpeedRead>, ICarSpeedRepositoryRead
	{
		public CarSpeedRepositoryRead(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

        public List<CarSpeedRead> GetAllOrdered(Guid CarId)
        {
            return GetAll().Where(o => o.CarId == CarId).OrderBy(o => o.SpeedTimeStamp).ToList();
        }
    }
}
