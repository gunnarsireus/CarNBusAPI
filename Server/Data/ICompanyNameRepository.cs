using Server.DAL;
using Shared.Models.Write;

namespace Server.Data
{
    public interface ICompanyNameRepository:IRepository<CompanyName>
    {
        ApiContext ApiContext { get; }
    }
}