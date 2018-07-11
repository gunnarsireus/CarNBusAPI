using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Shared.DAL;
using System;
using NServiceBus;
using Shared.Utils;
using System.IO;
using System.Linq;
using Shared.Messages.Commands;

namespace DeleteCompany
{
    public static class DeleteCompany
    {
        static readonly DataAccessRead _dataAccessRead = new DataAccessRead();
        static readonly IEndpointInstance _endpointInstance = Endpoint.Start(Helpers.CreateEndpoint(Helpers.ApiEndpoint, Directory.GetCurrentDirectory() + "\\App_Data")).GetAwaiter().GetResult();

        [FunctionName("DeleteCompany")]
        public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Company/{id}")]HttpRequest req, string id, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed DeleteCompany.");
            if (_dataAccessRead.GetCompany(new Guid(id)) == null) return;
            var companyId = new Guid(id);
            var cars = _dataAccessRead.GetCars().Where(c => c.CompanyId == companyId);
            foreach (var car in cars)
            {
                var deleteCar = new DeleteCar()
                {
                    CarId = car.CarId,
                    CompanyId = car.CompanyId,
                    DeleteCarTimeStamp = DateTime.Now.Ticks
                };
                _endpointInstance.Send(Helpers.ServerEndpoint, deleteCar).ConfigureAwait(false);
            }
            var message = new Shared.Messages.Commands.DeleteCompany
            {
                DataId = new Guid(),
                CompanyId = companyId,
                DeleteCompanyTimeStamp = DateTime.Now.Ticks
            };

            _endpointInstance.Send(Helpers.ServerEndpoint, message).ConfigureAwait(false);
        }
    }
}
