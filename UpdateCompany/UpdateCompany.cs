using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Shared.Models.Read;
using Shared.DAL;
using Shared.Messages.Commands;
using System;
using NServiceBus;
using Shared.Utils;
using Newtonsoft.Json;

namespace UpdateCompany
{
    public static class UpdateCompany
    {
        static readonly DataAccessRead _dataAccessRead = new DataAccessRead();
        static readonly IEndpointInstance _endpointInstance = Endpoint.Start(Helpers.CreateEndpoint(Helpers.ApiEndpoint, Directory.GetCurrentDirectory() + "\\App_Data")).GetAwaiter().GetResult();

        [FunctionName("UpdateCompany")]
        public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Company")]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed UpdateCompany.");
            //string company = req.Query["company"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var company = (CompanyRead)data?.company;

            var oldCompany = _dataAccessRead.GetCompany(company.CompanyId);
            if (oldCompany == null) return;
            var message = new UpdateCompanyName
            {
                DataId = new Guid(),
                CompanyId = company.CompanyId,
                Name = company.Name,
                UpdateCompanyNameTimeStamp = DateTime.Now.Ticks
            };

            _endpointInstance.Send(Helpers.ServerEndpoint, message).ConfigureAwait(false);
        }
    }
}
