using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Shared.Models;
using NServiceBus;
using Microsoft.AspNetCore.Cors;
using Messages.Commands;
using Microsoft.Extensions.Configuration;

namespace CarNBusAPI.Controllers
{
    [Route("api/[controller]")]
    public class CarController : Controller
    {
        readonly IEndpointInstance _endpointInstance;
        readonly DataAccess _dataAccess;
        public CarController(IEndpointInstance endpointInstance, IConfigurationRoot configuration)
        {
            _endpointInstance = endpointInstance;
            _dataAccess = new DataAccess(configuration);
        }
        // GET api/Car
        [HttpGet]
        [EnableCors("AllowAllOrigins")]
        public IEnumerable<Car> GetCars()
        {
            return _dataAccess.GetCars();
        }

        // GET api/Car/5
        [HttpGet("{id}")]
        [EnableCors("AllowAllOrigins")]
        public Car GetCar(string id)
        {
            return _dataAccess.GetCar(new Guid(id));
        }

        // POST api/Car
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        public void AddCar([FromBody] Car car)
        {
            var message = new CreateCar
            {
                CompanyId = car.CompanyId,

                _CarLockedStatus = new CarLockedStatus
                {
                    Locked = car._CarLockedStatus.Locked
                },
                _CarOnlineStatus = new CarOnlineStatus
                {
                    Online = car._CarOnlineStatus.Online
                },
                CreationTime = car.CreationTime,
                CarId = car.CarId,
                RegNr = car.RegNr,
                VIN = car.VIN
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        // PUT api/Car/5
        [HttpPut("{id}")]
        [EnableCors("AllowAllOrigins")]
        public void UpdateCar([FromBody] Car car)
        {
            if (GetCar(car.CarId.ToString()) == null) return;
            var message = new UpdateCarOnlineStatus
            {
                OnlineStatus = car._CarOnlineStatus.Online,
                CarId = car.CarId
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        // DELETE api/Car/5
        [HttpDelete("{id}")]
        [EnableCors("AllowAllOrigins")]
        public void DeleteCar(string id)
        {
            if (GetCar(id) == null) return;
            var message = new DeleteCar() { CarId = new Guid(id) };
            _endpointInstance.Send(message).ConfigureAwait(false);
        }
    }
}