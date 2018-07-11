using System;
using System.Collections.Generic;
using Shared.DAL;
using Shared.Models.Read;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Shared.Messages.Commands;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;

namespace CarNBusCarNBusAPI.Controllers
{
    [Route("")]
    public class DefaultController : Controller
    {
        [Route(""), HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public RedirectResult RedirectToSwaggerUi()
        {
            return Redirect("/swagger/");
        }
    }

    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        readonly IEndpointInstance _endpointInstance;
        readonly DataAccessWrite _dataAccessWrite;
        readonly DataAccessRead _dataAccessRead;

        public CompanyController(IEndpointInstance endpointInstance)
        {
            _endpointInstance = endpointInstance;
            _dataAccessWrite = new DataAccessWrite();
            _dataAccessRead = new DataAccessRead();
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
        public async Task AddCompany([FromBody] CompanyRead companyRead)
        {
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

            await _endpointInstance.Send("carnbusapi-server", createCompany).ConfigureAwait(false);
            await _endpointInstance.Send("carnbusapi-server", createCompanyName).ConfigureAwait(false);
            await _endpointInstance.Send("carnbusapi-server", createCompanyAddress).ConfigureAwait(false);
        }

        // PUT api/Company/5
        [EnableCors("AllowAllOrigins")]
        [HttpPut("/api/company/name/{id}")]
        public async Task UpdateCompanyName([FromBody] CompanyRead company)
        {
            var oldCompany = GetCompany(company.CompanyId.ToString());
            if (oldCompany == null) return;
            var message = new UpdateCompanyName
            {
                DataId = new Guid(),
                CompanyId = company.CompanyId,
                Name = company.Name,
                UpdateCompanyNameTimeStamp = DateTime.Now.Ticks
            };

            await _endpointInstance.Send("carnbusapi-server", message).ConfigureAwait(false);
        }

        // PUT api/Company/5
        [EnableCors("AllowAllOrigins")]
        [HttpPut("/api/company/address/{id}")]
        public async Task UpdateCompanyAddress([FromBody] CompanyRead company)
        {
            var oldCompany = GetCompany(company.CompanyId.ToString());
            if (oldCompany == null) return;
            var message = new UpdateCompanyAddress
            {
                DataId = new Guid(),
                CompanyId = company.CompanyId,
                Address = company.Address,
                UpdateCompanyAddressTimeStamp = DateTime.Now.Ticks
            };

            await _endpointInstance.Send("carnbusapi-server", message).ConfigureAwait(false);
        }

        // DELETE api/Company/5
        [HttpDelete("{id}")]
        public async Task DeleteCompany(string id)
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
                    DeleteCarTimeStamp = DateTime.Now.Ticks
                };
               await _endpointInstance.Send("carnbusapi-server", deleteCar).ConfigureAwait(false);
            }
            var message = new DeleteCompany
            {
                DataId = new Guid(),
                CompanyId = companyId,
                DeleteCompanyTimeStamp = DateTime.Now.Ticks
            };

            await _endpointInstance.Send("carnbusapi-server", message).ConfigureAwait(false);
        }
    }
}