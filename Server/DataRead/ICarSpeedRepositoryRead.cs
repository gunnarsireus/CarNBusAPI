using Shared.Models.Read;
using System.Collections.Generic;
using System;

namespace Server.DataRead
{
    public interface ICarSpeedRepositoryRead:IRepositoryRead<CarSpeedRead>
    {
        List<CarSpeedRead> GetAllOrdered(Guid CarId);
    }
}
