using Shared.Models;
using System;
using System.Collections.Generic;

namespace Server.Data
{
	public interface ICarOnlineStatusRepository:IRepository<CarOnlineStatus>
    {
        List<CarOnlineStatus> GetAllOrdered(Guid CarId);
    }
}
