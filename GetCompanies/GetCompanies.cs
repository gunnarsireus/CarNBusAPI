using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Shared.DAL;
using System.Collections.Generic;
using Shared.Models.Read;

namespace Company
{
    public static class GetCompanies
    {
        static readonly DataAccessRead _dataAccessRead = new DataAccessRead();
        // Azure Functions HTTP Trigger
        // Endpoint URL: /api/products/{id}
        [FunctionName("GetCompanies")]
        public static IEnumerable<CompanyRead> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed GetCompanies.");
            return _dataAccessRead.GetCompanies();
        }
    }
}
