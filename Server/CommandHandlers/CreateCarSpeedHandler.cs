using System.Threading.Tasks;
using Shared.Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models.Write;
using Shared.Models.Read;
using System;
using System.Globalization;

namespace Server.CommandHandlers
{
    public class CreateCarSpeedHandler : IHandleMessages<CreateCarSpeed>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does Create mean?
        public CreateCarSpeedHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<CreateCarSpeedHandler>();

        public Task Handle(CreateCarSpeed message, IMessageHandlerContext context)
        {

            log.Info("Received CreateCarSpeed");

            var carSpeed = new CarSpeed
            {
                Speed = message.Speed,
                CarId = message.CarId,
                SpeedTimeStamp = message.CreateCarSpeedTimeStamp
            };

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CarSpeeds.Add(carSpeed);
                unitOfWork.CarsReadNull.Add(new CarReadNull(message.CarId, message.CompanyId)
                {
                    Speed = message.Speed,
                    ChangeTimeStamp = message.CreateCarSpeedTimeStamp
                });
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}