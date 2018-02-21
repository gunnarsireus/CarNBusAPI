using Shared.Models.Write;
using Server.DAL;

namespace Server.Data
{
    public class CarLockedStatusRepository : Repository<CarLockedStatus>, ICarLockedStatusRepository
    {
		public CarLockedStatusRepository(ApiContext context) : base(context)
		{
		}

		public ApiContext ApiContext => Context as ApiContext;
    }
}
