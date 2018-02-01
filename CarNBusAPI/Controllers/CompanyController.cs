using System;
using System.Collections.Generic;
using Server.DAL;
using Shared.Models;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Messages.Commands;
using Microsoft.Extensions.Configuration;

namespace CarNBusAPI.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        readonly IEndpointInstance _endpointInstance;
        readonly DataAccess _dataAccess;
        public CompanyController(IEndpointInstance endpointInstance, IConfigurationRoot configuration)
        {
            _endpointInstance = endpointInstance;
            _dataAccess = new DataAccess(configuration);
        }

        // GET api/Company
        [HttpGet]
        public IEnumerable<Company> GetCompanies()
        {
            return _dataAccess.GetCompanies();
        }

        // GET api/Company/5
        [HttpGet("{id}")]
        public Company GetCompany(string id)
        {
            return _dataAccess.GetCompany(new Guid(id));
        }

        // POST api/Company
        [HttpPost]
        public void AddCompany([FromBody] Company company)
        {
            var message = new CreateCompany
            {
                DataId = new Guid(),
                Address = company.Address,
                Name = company.Name,
                CreationTime = company.CreationTime,
                Id = company.Id   //Todo check if new Guid?
            };
            // TODO: map object and massege

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        // PUT api/Company/5
        [HttpPut("{id}")]
        public void UpdateCompany([FromBody] Company company)
        {
            var message = new UpdateCompany
            {
                DataId = new Guid(),
                Id = company.Id,
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
            var message = new DeleteCompany
            {
                DataId = new Guid(),
                CompanyId = new Guid(id)
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }
    }
}