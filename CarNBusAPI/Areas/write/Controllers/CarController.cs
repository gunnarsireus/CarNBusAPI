using System;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Shared.Models.Read;
using NServiceBus;
using Microsoft.AspNetCore.Cors;
using Shared.Messages.Commands;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Shared.Messages.Events;

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
        public async Task AddCar([FromBody] CarRead carRead)
        {
            var createCar = new CreateCar
            {
                CompanyId = carRead.CompanyId,
                CreationTime = carRead.CreationTime,
                CarId = carRead.CarId,
                RegNr = carRead.RegNr,
                VIN = carRead.VIN,
                Locked = carRead.Locked,
                Online = carRead.Online,
                Speed = carRead.Speed,
                CreateCarTimeStamp = DateTime.Now.Ticks
            };

            var createOnlineStatus = new CreateCarOnlineStatus
            {
                CarId = carRead.CarId,
                CompanyId = carRead.CompanyId,
                OnlineStatus = carRead.Online,
                CreateCarOnlineTimeStamp = DateTime.Now.Ticks
            };

            var createLockedStatus = new CreateCarLockedStatus
            {
                CarId = carRead.CarId,
                CompanyId = carRead.CompanyId,
                LockedStatus = carRead.Locked,
                CreateCarLockedTimeStamp = DateTime.Now.Ticks
            };

            var createSpeed = new CreateCarSpeed
            {
                CarId = carRead.CarId,
                CompanyId = carRead.CompanyId,
                Speed = carRead.Speed,
                CreateCarSpeedTimeStamp = DateTime.Now.Ticks
            };
            await _endpointInstance.Send("carnbusapi-server", createCar).ConfigureAwait(false);
            await _endpointInstance.Send("carnbusapi-server", createOnlineStatus).ConfigureAwait(false);
            await _endpointInstance.Send("carnbusapi-server", createLockedStatus).ConfigureAwait(false);
            await _endpointInstance.Send("carnbusapi-server", createSpeed).ConfigureAwait(false);
        }
        // PUT api/Car/5
        [HttpPut("/api/write/car/online/{id}")]
        [EnableCors("AllowAllOrigins")]
        public async Task UpdateCarOnline([FromBody] CarRead CarRead)
        {
            var oldCar = GetCar(CarRead.CarId.ToString());
            if (oldCar == null) return;
            var message = new UpdateCarOnlineStatus
            {
                OnlineStatus = CarRead.Online,
                CarId = CarRead.CarId,
                CompanyId = CarRead.CompanyId,
                UpdateCarOnlineTimeStamp = DateTime.Now.Ticks
            };
            await _endpointInstance.Send("carnbusapi-server", message).ConfigureAwait(false);
        }

        [HttpPut("/api/write/car/locked/{id}")]
        [EnableCors("AllowAllOrigins")]
        public async Task UpdateCarLocked([FromBody] CarRead CarRead)
        {
            var oldCar = GetCar(CarRead.CarId.ToString());
            if (oldCar == null) return;
            var message = new UpdateCarLockedStatus
            {
                LockedStatus = CarRead.Locked,
                CarId = CarRead.CarId,
                CompanyId = CarRead.CompanyId,
                UpdateCarLockedTimeStamp = DateTime.Now.Ticks
            };
            await _endpointInstancePriority.Publish(message).ConfigureAwait(false);
        }

        [HttpPut("/api/write/car/speed/{id}")]
        [EnableCors("AllowAllOrigins")]
        public async Task UpdateCarSpeed([FromBody] CarRead CarRead)
        {
            var oldCar = GetCar(CarRead.CarId.ToString());
            if (oldCar == null) return;
            var message = new UpdateCarSpeed
            {
                Speed = CarRead.Speed,
                CarId = CarRead.CarId,
                CompanyId = CarRead.CompanyId,
                UpdateCarSpeedTimeStamp = DateTime.Now.Ticks
            };
            await _endpointInstance.Send("carnbusapi-server", message).ConfigureAwait(false);
        }

        // DELETE api/Car/5
        [HttpDelete("{id}")]
        [EnableCors("AllowAllOrigins")]
        public async Task DeleteCar(string id)
        {
            var oldCar = GetCar(id);
            if (oldCar == null) return;
            var message = new DeleteCar()
            {
                CarId = new Guid(id),
                CompanyId = oldCar.CompanyId,
                DeleteCarTimeStamp = DateTime.Now.Ticks
            };
            await _endpointInstance.Send("carnbusapi-server", message).ConfigureAwait(false);
        }

        CarRead GetCar(string id)
        {
            return _dataAccessRead.GetCar(new Guid(id));
        }
    }
}