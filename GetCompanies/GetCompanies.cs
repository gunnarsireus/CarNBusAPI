using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Shared.DAL;
using Shared.Models.Read;
using System.Collections.Generic;

namespace GetCompanies
{
    public static class GetCompanies
    {
        static readonly DataAccessRead _dataAccessRead = new DataAccessRead();

        [FunctionName("GetCompanies")]
        public static IEnumerable<CompanyRead> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed GetCompanyById.");
            return _dataAccessRead.GetCompanies();
        }
    }
}
