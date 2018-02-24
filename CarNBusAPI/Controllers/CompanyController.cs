using System;
using System.Collections.Generic;
using Server.DAL;
using Shared.Models.Read;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Messages.Commands;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace CarNBusAPI.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        readonly IEndpointInstance _endpointInstance;
        readonly DataAccessWrite _dataAccessWrite;
        readonly DataAccessRead _dataAccessRead;

        public CompanyController(IEndpointInstance endpointInstance, IConfigurationRoot configuration)
        {
            _endpointInstance = endpointInstance;
            _dataAccessWrite = new DataAccessWrite(configuration);
            _dataAccessRead = new DataAccessRead(configuration);
        }

        // GET api/Company
        [HttpGet]
        public IEnumerable<CompanyRead> GetCompanies()
        {
            return _dataAccessRead.GetCompanies();
        }

        // GET api/Company/5
        [HttpGet("{id}")]
        public CompanyRead GetCompany(string id)
        {
            return _dataAccessRead.GetCompany(new Guid(id));
        }

        // POST api/Company
        [HttpPost]
        public void AddCompany([FromBody] CompanyRead company)
        {
            var message = new CreateCompany
            {
                DataId = new Guid(),
                Address = company.Address,
                Name = company.Name,
                CreationTime = company.CreationTime,
                CompanyId = company.CompanyId
            };
            // TODO: map object and message

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        // PUT api/Company/5
        [HttpPut("{id}")]
        public void UpdateCompany([FromBody] CompanyRead company)
        {
            if (GetCompany(company.CompanyId.ToString()) == null) return;
            var message = new UpdateCompany
            {
                DataId = new Guid(),
                Id = company.CompanyId,
                Address = company.Address,
                CreationTime = company.CreationTime,
                Name = company.Name
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        // DELETE api/Company/5
        [HttpDelete("{id}")]
        public void DeleteCompany(string id)
        {
            if (GetCompany(id) == null) return;
            var companyId = new Guid(id);
            var cars = _dataAccessRead.GetCars().Where(c => c.CompanyId == companyId);
            foreach (var car in cars)
            {
                var deleteCar = new DeleteCar()
                {
                    CarId = car.CarId,
                    CompanyId = car.CompanyId,
                    DeleteTimeStamp = DateTime.Now.Ticks
                };
                _endpointInstance.Send(deleteCar).ConfigureAwait(false);
            }
            var message = new DeleteCompany
            {
                DataId = new Guid(),
                CompanyId = companyId,
                DeleteTimeStamp = DateTime.Now.Ticks
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }
    }
}