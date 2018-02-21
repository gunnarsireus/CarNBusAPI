using Shared.Models.Write;
using Server.DAL;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Server.Data
{
	public class CarRepository : Repository<Car>, ICarRepository
	{
		public CarRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;
    }
}
