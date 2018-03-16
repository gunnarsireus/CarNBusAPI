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
    public class UpdateCarSpeedHandler : IHandleMessages<UpdateCarSpeed>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does update mean?
        public UpdateCarSpeedHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<UpdateCarSpeedHandler>();

        public Task Handle(UpdateCarSpeed message, IMessageHandlerContext context)
        {

            log.Info("Received UpdateCarSpeed");

            var carSpeed = new CarSpeed
            {
                Speed = message.Speed,
                CarId = message.CarId,
                SpeedTimeStamp = message.UpdateCarSpeedTimeStamp
            };

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CarSpeeds.Update(carSpeed);
                unitOfWork.CarsReadNull.Add(new CarReadNull(message.CarId, message.CompanyId)
                {
                    Speed = message.Speed,
                    ChangeTimeStamp = message.UpdateCarSpeedTimeStamp
                });
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}