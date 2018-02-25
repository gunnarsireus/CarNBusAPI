using System.Threading.Tasks;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models.Write;
using Shared.Models.Read;
using System;

namespace Server.CommandHandlers
{
    public class CreateCarLockedStatusHandler : IHandleMessages<CreateCarLockedStatus>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does update mean?
        public CreateCarLockedStatusHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<CreateCarLockedStatusHandler>();

        public Task Handle(CreateCarLockedStatus message, IMessageHandlerContext context)
        {

            log.Info("Received CreateCarLockedStatus");

            var carLockedStatus = new CarLockedStatus
            {
                Locked = message.LockedStatus,
                CarId = message.CarId,
                LockedTimeStamp = message.CreateCarLockedTimeStamp
            };
            var carLockedStatusRead = new CarLockedStatusRead
            {
                Locked = message.LockedStatus,
                CarId = message.CarId,
                LockedTimeStamp = message.CreateCarLockedTimeStamp
            };

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CarLockedStatuses.Add(carLockedStatus);
                unitOfWork.CarsReadNull.Add(new CarReadNull(message.CarId,message.CompanyId)
                {
                    Locked = message.LockedStatus,
                    ChangeTimeStamp = message.CreateCarLockedTimeStamp

                });
                unitOfWork.Complete();
            }
            return Task.CompletedTask;
        }
    }
}