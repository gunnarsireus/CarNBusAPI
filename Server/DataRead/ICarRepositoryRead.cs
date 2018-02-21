using Shared.Models.Read;
using System.Collections.Generic;
using System;

namespace Server.DataRead
{
    public interface ICarRepositoryRead:IRepositoryRead<CarRead>
    {
        List<CarRead> GetAllByCompanyId(Guid CompanyId);
    }
}
