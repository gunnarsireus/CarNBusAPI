using Server.DAL;
using Shared.Models.Write;

namespace Server.Data
{
    public interface ICarOnlineStatusRepository : IRepository<CarOnlineStatus>
    {
        ApiContext ApiContext { get; }
    }
}