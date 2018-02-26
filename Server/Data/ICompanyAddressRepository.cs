using Server.DAL;
using Shared.Models.Write;

namespace Server.Data
{
    public interface ICompanyAddressRepository:IRepository<CompanyAddress>
    {
        ApiContext ApiContext { get; }
    }
}