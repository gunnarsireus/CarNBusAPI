using System.Threading.Tasks;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models;
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

            var CarSpeed = new CarSpeed();
            CarSpeed.Speed = message.Speed;
            CarSpeed.CarId = message.CarId;

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CarSpeed.Update(CarSpeed);
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}