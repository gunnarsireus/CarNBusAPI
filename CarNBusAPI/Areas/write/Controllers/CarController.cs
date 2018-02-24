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
        readonly DataAccessWrite _dataAccessWrite;
        readonly DataAccessRead _dataAccessRead;
        public CarController(IEndpointInstance endpointInstance, IEndpointInstance endpointInstancePriority, IConfigurationRoot configuration)
        {
            _endpointInstance = endpointInstance;
            _endpointInstancePriority = endpointInstancePriority;
            _dataAccessWrite = new DataAccessWrite(configuration);
            _dataAccessRead = new DataAccessRead(configuration);
        }

        // POST api/Car
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        public void AddCar([FromBody] CarRead carRead)
        {
            var message = new CreateCar
            {
                CompanyId = carRead.CompanyId,
                CreationTime = carRead.CreationTime,
                CarId = carRead.CarId,
                RegNr = carRead.RegNr,
                VIN = carRead.VIN
            };
            //todo: create messages for Online, Locked and Speed
            var createOnlineStatus = new CreateCarOnlineStatus
            {
                CarId = carRead.CarId,
                CompanyId = carRead.CompanyId,
                OnlineStatus = carRead.Online,
                CreationTime = carRead.ChangedTimeStamp
            };

            var createLockedStatus = new CreateCarLockedStatus
            {
                CarId = carRead.CarId,
                CompanyId = carRead.CompanyId,
                LockedStatus = carRead.Locked,
                LockedTimeStamp = carRead.LockedTimeStamp,
            };

            var createSpeed = new CreateCarSpeed
            {
                CarId = carRead.CarId,
                CompanyId = carRead.CompanyId,
                Speed = carRead.Speed,
                CreationTime = carRead.ChangedTimeStamp
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
            _endpointInstance.Send(createOnlineStatus).ConfigureAwait(false);
            _endpointInstance.Send(createLockedStatus).ConfigureAwait(false);
            _endpointInstance.Send(createSpeed).ConfigureAwait(false);
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
                CompanyId = CarRead.CompanyId,
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
                CompanyId = CarRead.CompanyId,
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
                CompanyId = CarRead.CompanyId,
                SpeedTimeStamp = DateTime.Now.Ticks
            };

            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        // DELETE api/Car/5
        [HttpDelete("{id}")]
        [EnableCors("AllowAllOrigins")]
        public void DeleteCar(string id)
        {
            var oldCar = GetCar(id);
            var message = new DeleteCar()
            {
                CarId = new Guid(id),
                CompanyId = oldCar.CompanyId,
                DeleteTimeStamp = DateTime.Now.Ticks
            };
            _endpointInstance.Send(message).ConfigureAwait(false);
        }

        CarRead GetCar(string id)
        {
            return _dataAccessRead.GetCar(new Guid(id));
        }
    }
}