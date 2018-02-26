using Shared.Models.Read;
using System.Collections.Generic;
using System;

namespace Server.DataRead
{
    public interface ICompanyNameRepositoryRead:IRepositoryRead<CompanyNameRead>
    {
        List<CompanyNameRead> GetAllOrdered(Guid CarId);
    }
}
