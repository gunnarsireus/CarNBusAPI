using Shared.Models;
using System;
using System.Collections.Generic;

namespace Server.Data
{
	public interface ICarSpeedRepository:IRepository<CarSpeed>
    {
        List<CarSpeed> GetAllOrdered(Guid CarId);
    }
}
