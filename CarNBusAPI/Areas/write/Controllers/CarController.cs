using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Shared.Models.Read;
using Shared.Models.Write;
using NServiceBus;
using Microsoft.AspNetCore.Cors;
using Messages.Commands;
using Microsoft.Extensions.Configuration;

namespace CarNBusAPI.Write.Controllers
{
    [Route("api/write/[controller]")]
    public class CarController : Controller
    {
        readonly IEndpointInstance _endpointInstance;
        readonly IEndpointInstance _endpointInstancePriority;
        readonly DataAccessWrite _dataAccess;
        public CarController(IEndpointInstance endpointInstance, IEndpointInstance endpointInstancePriority, IConfigurationRoot configuration)
        {
            _endpointInstance = endpointInstance;
            _endpointInstancePriority = endpointInstancePriority;
            _dataAccess = new DataAccessWrite(configuration);
        }

        // POST api/Car
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        public void AddCar([FromBody] CarRead carRead)
        {
            var message = new CreateCar
            {
                CompanyId = carRead.CompanyId,
                _CarLockedStatus = carRead.Locked,
                LockedTimeStamp = carRead.LockedTimeStamp,
                _CarOnlineStatus = carRead.Online,
                CreationTime = carRead.CreationTime,
                CarId = carRead.CarId,
                RegNr = carRead.RegNr,
                VIN = carRead.VIN
            };
            //todo: create messages for Online, Locked and Speed
            _endpointInstance.Send(message).ConfigureAwait(false);
        }
        // Todo: Separeta to UpdateOnline, UpdateLocked and UpdateSpeed
        // PUT api/Car/5
        [HttpPut("/api/write/car/online/{id}")]
        [EnableCors("AllowAllOrigins")]
        public void UpdateCarOnline([FromBody] CarRead CarRead)
        {
            var message = new UpdateCarOnlineStatus
            {
                OnlineStatus = CarRead.Online,
                CarId = CarRead.CarId,
                OnlineTimeStamp = DateTime.Now.Ticks
        };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        [HttpPut("/api/write/car/locked/{id}")]
        [EnableCors("AllowAllOrigins")]
        public void UpdateCarLocked([FromBody] CarRead CarRead)
        {
            var message = new UpdateCarLockedStatus
            {
                LockedStatus = CarRead.Locked,
                CarId = CarRead.CarId,
                LockedTimeStamp = DateTime.Now.Ticks
            };

            _endpointInstancePriority.Send(message).ConfigureAwait(false);
        }

        [HttpPut("/api/write/car/speed/{id}")]
        [EnableCors("AllowAllOrigins")]
        public void UpdateCarSpeed([FromBody] CarRead CarRead)
        {
            var message = new UpdateCarSpeed
            {
                Speed = CarRead.Speed,
                CarId = CarRead.CarId,
                SpeedTimeStamp = DateTime.Now.Ticks
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        // DELETE api/Car/5
        [HttpDelete("{id}")]
        [EnableCors("AllowAllOrigins")]
        public void DeleteCar(string id)
        {
            var message = new DeleteCar() { CarId = new Guid(id) };
            _endpointInstance.Send(message).ConfigureAwait(false);
        }
    }
}