using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Shared.DAL;
using Shared.Models.Read;
using System;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Shared.Messages.Commands;
using Shared.Utils;
using System.IO;

namespace AddCompany
{
    public static class AddCompany
    {
        static readonly DataAccessWrite _dataAccessWrite = new DataAccessWrite();
        static readonly IEndpointInstance _endpointInstance = Endpoint.Start(Helpers.CreateEndpoint(Helpers.ApiEndpoint, Directory.GetCurrentDirectory() + "\\App_Data")).GetAwaiter().GetResult();

        [FunctionName("AddCompany")]
        public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Company")]HttpRequest req, [FromBody] CompanyRead companyRead, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed AddCompany.");

            var createCompany = new CreateCompany
            {
                DataId = new Guid(),
                Address = companyRead.Address,
                Name = companyRead.Name,
                CreationTime = companyRead.CreationTime,
                CompanyId = companyRead.CompanyId,
                CreateCompanyTimeStamp = DateTime.Now.Ticks
            };
            var createCompanyName = new CreateCompanyName
            {
                CompanyId = companyRead.CompanyId,
                Name = companyRead.Name,
                CreateCompanyNameTimeStamp = DateTime.Now.Ticks
            };

            var createCompanyAddress = new CreateCompanyAddress
            {
                CompanyId = companyRead.CompanyId,
                Address = companyRead.Address,
                CreateCompanyAddressTimeStamp = DateTime.Now.Ticks
            };

            _endpointInstance.Send(Helpers.ServerEndpoint, createCompany).ConfigureAwait(false);
            _endpointInstance.Send(Helpers.ServerEndpoint, createCompanyName).ConfigureAwait(false);
            _endpointInstance.Send(Helpers.ServerEndpoint, createCompanyAddress).ConfigureAwait(false);
        }
    }
}
