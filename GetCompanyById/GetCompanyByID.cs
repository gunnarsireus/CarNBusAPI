using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Shared.DAL;
using Shared.Models.Read;
using System;

namespace GetCompanyById
{
    public static class GetCompanyByID
    {
        // Azure Functions HTTP Trigger
        // Endpoint URL: /api/products/{id}
        static readonly DataAccessRead _dataAccessRead = new DataAccessRead();
        [FunctionName("GetCompanyById")]
        public static CompanyRead Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Company/{id}")]HttpRequest req, string id, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed GetCompanyById.");
            return _dataAccessRead.GetCompany(new Guid(id));
        }
    }
}
