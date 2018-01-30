using CarNBusAPI.DAL;
using CarNBusAPI.Models;

namespace CarNBusAPI.Data
{
	public class CarRepository : Repository<Car>, ICarRepository
	{
		public CarRepository(CarNBusAPIContext context) : base(context)
		{
		}

		public CarNBusAPIContext CarNBusAPIContext => Context as CarNBusAPIContext;

	}
}
