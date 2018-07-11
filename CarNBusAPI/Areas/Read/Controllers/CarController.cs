using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shared.DAL;
using Shared.Models.Read;
using NServiceBus;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Shared.Utils;
using Shared.Messages.Events;

namespace CarNBusAPI.Read.Controllers
{
    [Route("api/read/[controller]")]
    public class CarController : Controller
    {
        readonly IEndpointInstance _endpointInstance;
        readonly IEndpointInstance _endpointInstancePriority;
        readonly DataAccessRead _dataAccess;

        public CarController(IEndpointInstance endpointInstance, IEndpointInstance endpointInstancePriority)
        {
            _endpointInstance = endpointInstance;
            _endpointInstancePriority = endpointInstancePriority;
            _dataAccess = new DataAccessRead();
        }

        private static bool CarIdAlreadyInQueue(Dictionary<string, int> guidQueueLength, string carId)
        {
            return guidQueueLength.Any(a => a.Key == carId);
        }

        private static bool CarHasBeenLocked40Seconds(CarRead car)
        {
            return new DateTime(car.LockedTimeStamp).AddMilliseconds(40000) < DateTime.Now;
        }

        // GET api/Car
        [HttpGet]
        [EnableCors("AllowAllOrigins")]
        public async Task<IEnumerable<CarRead>> GetCars()
        {
            var list = new List<CarRead>();
            var cars = _dataAccess.GetCars();
            foreach (var car in cars)
            {
                if (car.Locked)
                {
                    if (CarHasBeenLocked40Seconds(car))
                    {  //Lock timed out and can be ignored and set to false
                        var updateCarLockedStatus = new UpdateCarLockedStatus
                        {
                            LockedStatus = false,
                            CarId = car.CarId,
                            CompanyId = car.CompanyId,
                            UpdateCarLockedTimeStamp = DateTime.Now.Ticks
                        };

                        await _endpointInstancePriority.Publish(updateCarLockedStatus).ConfigureAwait(false);
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

        static string GetCarIdFromMessage(CloudQueueMessage message)
        {
            var json = message.AsString;
            var byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (json.StartsWith(json))
            {
                json = json.Remove(0, byteOrderMarkUtf8.Length);
            }
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            var body = (string)parsedJson.Body;
            byte[] data = Convert.FromBase64String(body);
            string tmp = Encoding.UTF8.GetString(data);
            if (tmp.IndexOf("CarId") > -1)
            {
                return tmp.Substring(tmp.IndexOf("CarId") + 8, 36);
            }
            return Guid.Empty.ToString();
        }

        static async Task<Dictionary<string, int>> GetQueueLenghtForEachCar()
        {
            Dictionary<string, int> queueLengthPerCar = new Dictionary<string, int>();
            CloudStorageAccount storageAccount = QueueStorage.Common.CreateStorageAccountFromConnectionString(Helpers.GetStorageConnection());
            CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = cloudQueueClient.GetQueueReference(Helpers.ServerEndpoint);
            IEnumerable<CloudQueueMessage> peekedMessages = await queue.PeekMessagesAsync(32);
            if (peekedMessages != null)
            {
                foreach (var msg in peekedMessages.ToList())
                {
                    var carId = GetCarIdFromMessage(msg);
                    if (CarIdAlreadyInQueue(queueLengthPerCar, carId))
                    {
                        queueLengthPerCar[carId]++;
                    }
                    else
                    {
                        queueLengthPerCar.Add(carId, 1);
                    }
                }
            }
            return queueLengthPerCar;
        }

        [HttpGet("/api/read/carandqueuelength")]
        [EnableCors("AllowAllOrigins")]
        public async Task<IEnumerable<CarRead>> GetCarsAndQueLengthAsync()
        {
            Dictionary<string, int> queueLengthForEachCar = await GetQueueLenghtForEachCar();
            var list = new List<CarRead>();
            var cars = _dataAccess.GetCars();
            foreach (var car in cars)
            {
                if (car.Locked)
                {
                    if (CarHasBeenLocked40Seconds(car))
                    {  //Lock timed out and can be ignored and set to false
                        var updateCarLockedStatus = new UpdateCarLockedStatus
                        {
                            LockedStatus = false,
                            CarId = car.CarId,
                            CompanyId = car.CompanyId,
                            UpdateCarLockedTimeStamp = DateTime.Now.Ticks
                        };

                        await _endpointInstancePriority.Publish(updateCarLockedStatus).ConfigureAwait(false);
                    }
                }
                int tmpQueueLenghtForCar = 0;
                if (CarIdAlreadyInQueue(queueLengthForEachCar, car.CarId.ToString()))
                {
                    tmpQueueLenghtForCar = queueLengthForEachCar.FirstOrDefault(k => k.Key == car.CarId.ToString()).Value;
                };

                list.Add(new CarRead(car.CarId)
                {
                    CompanyId = car.CompanyId,
                    CreationTime = car.CreationTime,
                    Locked = car.Locked,
                    Online = car.Online,
                    Speed = car.Speed,
                    RegNr = car.RegNr,
                    VIN = car.VIN,
                    QueueLength = tmpQueueLenghtForCar
                });
            }
            return list;
        }

        // GET api/Car/5
        [HttpGet("{id}")]
        [EnableCors("AllowAllOrigins")]
        public async Task<CarRead> GetCar(string id)
        {
            var car = _dataAccess.GetCar(new Guid(id));
            if (car.Locked)
            {
                if (CarHasBeenLocked40Seconds(car))
                {  //Lock timed out and can be ignored and set to false
                    var updateCarLockedStatus = new UpdateCarLockedStatus
                    {
                        LockedStatus = false,
                        CarId = car.CarId,
                        CompanyId = car.CompanyId,
                        UpdateCarLockedTimeStamp = DateTime.Now.Ticks
                    };

                    await _endpointInstancePriority.Publish(updateCarLockedStatus).ConfigureAwait(false);
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