using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Shared.Models;
using NServiceBus;
using Microsoft.AspNetCore.Cors;
using Messages.Commands;
using Microsoft.Extensions.Configuration;

namespace CarNBusAPI.Read.Controllers
{
    [Route("api/read/[controller]")]
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
        // GET api/Car
        [HttpGet]
        [EnableCors("AllowAllOrigins")]
        public IEnumerable<ClientCar> GetCars()
        {
            var cars = _dataAccess.GetCars();
            var list = new List<ClientCar>();
            foreach (var car in cars)
            {
                car.CarOnlineStatuses = _dataAccess.GetCarOnlineStatuses(car.CarId);
                car.CarLockedStatuses = _dataAccess.GetCarLockedStatuses(car.CarId);
                car.CarSpeeds = _dataAccess.GetCarSpeeds(car.CarId);
                if (car.Locked)
                {
                    if (new DateTime(car.LockedTimeStamp).AddMilliseconds(20000) < DateTime.Now)
                    {  //Lock timeouted can be ignored and set to false
                        var message = new UpdateCarLockedStatus
                        {
                            LockedStatus = false,
                            CarId = car.CarId
                        };

                        _endpointInstancePriority.Send(message).ConfigureAwait(false);
                        car.Locked = false;
                    }
                }

                list.Add(new ClientCar
                {
                    CarId = car.CarId,
                    CompanyId = car.CompanyId,
                    CreationTime = car.CreationTime,
                    Locked = car.Locked,
                    Online = car.Online,
                    Speed = car.Speed,
                    RegNr = car.RegNr,
                    VIN = car.VIN
                });
            }
            return list;
        }

        // GET api/Car/5
        [HttpGet("{id}")]
        [EnableCors("AllowAllOrigins")]
        public ClientCar GetCar(string id)
        {
            var car = _dataAccess.GetCar(new Guid(id));
            car.CarOnlineStatuses = _dataAccess.GetCarOnlineStatuses(car.CarId);
            car.CarLockedStatuses = _dataAccess.GetCarLockedStatuses(car.CarId);
            car.CarSpeeds = _dataAccess.GetCarSpeeds(car.CarId);
            if (car.Locked)
            {
                if (new DateTime(car.LockedTimeStamp).AddMilliseconds(20000) < DateTime.Now)
                {  //Lock timeouted can be ignored and set to false
                    var message = new UpdateCarLockedStatus
                    {
                        LockedStatus = false,
                        CarId = car.CarId
                    };

                    _endpointInstancePriority.Send(message).ConfigureAwait(false);
                    car.Locked = false;
                }
            }

            var clientCar = new ClientCar
            {
                CarId = car.CarId,
                CompanyId = car.CompanyId,
                CreationTime = car.CreationTime,
                Locked = car.Locked,
                Online = car.Online,
                Speed = car.Speed,
                RegNr = car.RegNr,
                VIN = car.VIN
            };
            return clientCar;
        }
    }
}