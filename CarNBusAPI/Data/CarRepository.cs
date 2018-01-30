using CarNBusAPI.DAL;
using CarNBusAPI.Models;

namespace CarNBusAPI.Data
{
	public class CarRepository : Repository<Car>, ICarRepository
	{
		public CarRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

	}
}
