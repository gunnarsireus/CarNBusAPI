using Shared.Models.Read;
using System.Collections.Generic;
using System;

namespace Server.DataRead
{
    public interface ICompanyAddressRepositoryRead:IRepositoryRead<CompanyAddressRead>
    {
        List<CompanyAddressRead> GetAllOrdered(Guid CarId);
    }
}
