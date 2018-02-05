namespace Server.CommandHandlers
{
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

    public class UpdateCarLockedStatusHandler : IHandleMessages<UpdateCarLockedStatus>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does update mean?
        public UpdateCarLockedStatusHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<UpdateCarLockedStatusHandler>();

        public Task Handle(UpdateCarLockedStatus message, IMessageHandlerContext context)
        {

            log.Info("Received UpdateCarLockedStatus");

            var carLockedStatus = new CarLockedStatus();
            carLockedStatus.Locked = message.LockedStatus;
            carLockedStatus.CarId = message.CarId;
            carLockedStatus.LockedTimeStamp = DateTime.Now.Ticks;

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CarLockedStatus.Update(carLockedStatus);
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}