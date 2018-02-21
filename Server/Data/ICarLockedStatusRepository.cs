using Server.DAL;
using Shared.Models.Write;

namespace Server.Data
{
    public interface ICarLockedStatusRepository:IRepository<CarLockedStatus>
    {
        ApiContext ApiContext { get; }
    }
}