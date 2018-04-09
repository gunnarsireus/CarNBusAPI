using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.DAL;
using Shared.Models.Read;
using NServiceBus;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Shared.Messages.Commands;
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
            var list = new List<CarRead>();
            var cars = _dataAccess.GetCars();
            foreach (var car in cars)
            {
                if (car.Locked)
                {
                    if (new DateTime(car.LockedTimeStamp).AddMilliseconds(20000) < DateTime.Now)
                    {  //Lock timeouted can be ignored and set to false
                        var message = new UpdateCarLockedStatus
                        {
                            LockedStatus = false,
                            CarId = car.CarId,
                            CompanyId = car.CompanyId,
                            UpdateCarLockedTimeStamp = DateTime.Now.Ticks
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

        IDictionary<string, int> GetCarsAndQueueLenght()
        {
            var list = new Dictionary<string, int>
            {
                { "ABC123", 7 },
                { "DEF456", 3 },
                { "GHI789", 4 },
                { "JKL012", 2 },
                { "MNO345", 9 },
                { "PQR678", 3 },
                { "STU901", 3 }
            };
            return list;
        }

        [HttpGet("/api/read/carandqueuelength")]
        [EnableCors("AllowAllOrigins")]
        public IEnumerable<CarRead> GetCarsAndQueLength()
        {
            var carsAndQueueLength = GetCarsAndQueueLenght();
            var list = new List<CarRead>();
            var cars = _dataAccess.GetCars();
            foreach (var car in cars)
            {
                if (car.Locked)
                {
                    if (new DateTime(car.LockedTimeStamp).AddMilliseconds(20000) < DateTime.Now)
                    {  //Lock timeouted can be ignored and set to false
                        var message = new UpdateCarLockedStatus
                        {
                            LockedStatus = false,
                            CarId = car.CarId,
                            CompanyId = car.CompanyId,
                            UpdateCarLockedTimeStamp = DateTime.Now.Ticks
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
                    VIN = car.VIN,
                    QueueLength = carsAndQueueLength.FirstOrDefault(k => k.Key == car.RegNr).Value
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
                if (new DateTime(car.LockedTimeStamp).AddMilliseconds(20000) < DateTime.Now)
                {  //Lock timeouted can be ignored and set to false
                    var message = new UpdateCarLockedStatus
                    {
                        LockedStatus = false,
                        CarId = car.CarId,
                        CompanyId = car.CompanyId,
                        UpdateCarLockedTimeStamp = DateTime.Now.Ticks
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