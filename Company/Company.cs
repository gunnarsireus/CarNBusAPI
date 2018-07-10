
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Server.DAL;
using System.Collections.Generic;
using Shared.Models.Read;

namespace Company
{
    public static class Company
    {
        static readonly DataAccessRead _dataAccessRead = new DataAccessRead();
        [FunctionName("Company")]
        public static IEnumerable<CompanyRead> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed GetCompanies.");
            return _dataAccessRead.GetCompanies();
        }
    }
}
