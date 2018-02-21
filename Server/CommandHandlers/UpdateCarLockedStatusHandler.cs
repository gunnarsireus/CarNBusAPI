using System.Threading.Tasks;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models.Write;
using System.Linq;
using System.Globalization;

namespace Server.CommandHandlers
{
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

            var carLockedStatus = new CarLockedStatus
            {
                Locked = message.LockedStatus,
                CarId = message.CarId,
                LockedTimeStamp = message.LockedTimeStamp
            };

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CarLockedStatuses.Update(carLockedStatus);
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}