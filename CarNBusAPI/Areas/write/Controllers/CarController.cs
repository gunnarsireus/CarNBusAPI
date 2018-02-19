using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Shared.Models;
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
        readonly DataAccess _dataAccess;
        public CarController(IEndpointInstance endpointInstance, IEndpointInstance endpointInstancePriority, IConfigurationRoot configuration)
        {
            _endpointInstance = endpointInstance;
            _endpointInstancePriority = endpointInstancePriority;
            _dataAccess = new DataAccess(configuration);
        }

        // POST api/Car
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        public void AddCar([FromBody] ClientCar clientCar)
        {
            var message = new CreateCar
            {
                CompanyId = clientCar.CompanyId,
                _CarLockedStatus = clientCar.Locked,
                _CarOnlineStatus = clientCar.Online,
                CreationTime = clientCar.CreationTime,
                CarId = clientCar.CarId,
                RegNr = clientCar.RegNr,
                VIN = clientCar.VIN
            };
            //todo: create messages for Online, Locked and Speed
            _endpointInstance.Send(message).ConfigureAwait(false);
        }
        // Todo: Separeta to UpdateOnline, UpdateLocked and UpdateSpeed
        // PUT api/Car/5
        [HttpPut("/api/write/Car/online/{id}")]
        [EnableCors("AllowAllOrigins")]
        public void UpdateCarOnline([FromBody] ClientCar clientCar)
        {
            var message = new UpdateCarOnlineStatus
            {
                OnlineStatus = clientCar.Online,
                CarId = clientCar.CarId
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        [HttpPut("/api/write/Car/locked/{id}")]
        [EnableCors("AllowAllOrigins")]
        public void UpdateCarLocked([FromBody] ClientCar clientCar)
        {
            var message = new UpdateCarLockedStatus
            {
                LockedStatus = clientCar.Locked,
                CarId = clientCar.CarId
            };

            _endpointInstancePriority.Send(message).ConfigureAwait(false);
        }

        [HttpPut("/api/write/Car/speed/{id}")]
        [EnableCors("AllowAllOrigins")]
        public void UpdateCarSpeed([FromBody] ClientCar clientCar)
        {
            var message = new UpdateCarSpeed
            {
                Speed = clientCar.Speed,
                CarId = clientCar.CarId
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