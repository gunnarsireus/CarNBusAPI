using Shared.Models.Read;
using System.Collections.Generic;
using System;

namespace Server.DataRead
{
    public interface ICarOnlineStatusRepositoryRead:IRepositoryRead<CarOnlineStatusRead>
    {
        List<CarOnlineStatusRead> GetAllOrdered(Guid CarId);
    }
}
