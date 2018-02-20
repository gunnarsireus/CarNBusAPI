using Shared.Models;
using System;
using System.Collections.Generic;

namespace Server.Data
{
	public interface ICarRepository:IRepository<Car>
    {
        List<Car> GetAllByCompanyId(Guid CompanyId);
    }
}
