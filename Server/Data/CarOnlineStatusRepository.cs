using Shared.Models;
using Server.DAL;

namespace Server.Data
{
	public class CarOnlineStatusRepository : Repository<CarOnlineStatus>, ICarOnlineStatusRepository
	{
		public CarOnlineStatusRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;

	}
}
