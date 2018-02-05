using Shared.Models;
using Server.DAL;

namespace Server.Data
{
	public class CarSpeedRepository : Repository<CarSpeed>, ICarSpeedRepository
	{
		public CarSpeedRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

	}
}
