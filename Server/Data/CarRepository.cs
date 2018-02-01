using Shared.Models;
using Server.DAL;

namespace Server.Data
{
	public class CarRepository : Repository<Car>, ICarRepository
	{
		public CarRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext CarApiContext => Context as ApiContext;

	}
}
