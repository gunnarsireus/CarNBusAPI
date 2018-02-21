using Server.DAL;
using Shared.Models.Write;

namespace Server.Data
{
    public interface ICarSpeedRepository : IRepository<CarSpeed>
    {
        ApiContext ApiContext { get; }
    }
}