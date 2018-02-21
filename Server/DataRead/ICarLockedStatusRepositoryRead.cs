using System.Collections.Generic;
using System;
using Shared.Models.Read;

namespace Server.DataRead
{
    public interface ICarLockedStatusRepositoryRead : IRepositoryRead<CarLockedStatusRead>
    {
        List<CarLockedStatusRead> GetAllOrdered(Guid CarId);
    }
}
