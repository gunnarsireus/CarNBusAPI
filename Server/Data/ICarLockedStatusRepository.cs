using Shared.Models;
using System;
using System.Collections.Generic;

namespace Server.Data
{
    public interface ICarLockedStatusRepository : IRepository<CarLockedStatus>
    {
        List<CarLockedStatus> GetAllOrdered(Guid CarId);
    }
}
