using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Shared.Models.Read;
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
        readonly DataAccessRead _dataAccess;
        public CarController(IEndpointInstance endpointInstance, IEndpointInstance endpointInstancePriority, IConfigurationRoot configuration)
        {
            _endpointInstance = endpointInstance;
            _endpointInstancePriority = endpointInstancePriority;
            _dataAccess = new DataAccessRead(configuration);
        }
        // GET api/Car
        [HttpGet]
        [EnableCors("AllowAllOrigins")]
        public IEnumerable<CarRead> GetCars()
        {
            var cars = _dataAccess.GetCars();
            var list = new List<CarRead>();
            foreach (var car in cars)
            {
                if (car.Locked)
                {
                    if (new DateTime(car.ChangeTimeStamp).AddMilliseconds(20000) < DateTime.Now)
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

                list.Add(new CarRead(car.CarId)
                {
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
        public CarRead GetCar(string id)
        {
            var car = _dataAccess.GetCar(new Guid(id));
            if (car.Locked)
            {
                if (new DateTime(car.ChangeTimeStamp).AddMilliseconds(20000) < DateTime.Now)
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

            var CarRead = new CarRead(car.CarId)
            {
                CompanyId = car.CompanyId,
                CreationTime = car.CreationTime,
                Locked = car.Locked,
                Online = car.Online,
                Speed = car.Speed,
                RegNr = car.RegNr,
                VIN = car.VIN
            };
            return CarRead;
        }
    }
}