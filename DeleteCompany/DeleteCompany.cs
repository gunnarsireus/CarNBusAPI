using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Shared.DAL;
using Shared.Models.Read;
using System;

namespace DeleteCompany
{
    public static class DeleteCompany
    {
        static readonly DataAccessWrite _dataAccessWrite = new DataAccessWrite();
        [FunctionName("DeleteCompany")]
        public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Company/{id}")]HttpRequest req, string id, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed DeleteCompany.");
            _dataAccessWrite.DeleteCompany(new Guid(id));
        }
    }
}
