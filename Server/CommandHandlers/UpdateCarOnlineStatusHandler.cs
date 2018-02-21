namespace Server.CommandHandlers
{
    using System.Threading.Tasks;
    using Messages.Commands;
    using Microsoft.EntityFrameworkCore;
    using NServiceBus;
    using NServiceBus.Logging;
    using Server.Data;
    using Server.DAL;
    using Shared.Models.Write;

    public class UpdateCarOnlineStatusHandler : IHandleMessages<UpdateCarOnlineStatus>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does update mean?
        public UpdateCarOnlineStatusHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<UpdateCarOnlineStatusHandler>();

        public Task Handle(UpdateCarOnlineStatus message, IMessageHandlerContext context)
        {
            log.Info("Received UpdateCarOnlineStatus");

            var carOnlineStatus = new CarOnlineStatus
            {
                Online = message.OnlineStatus,
                CarId = message.CarId,
                OnlineTimeStamp = message.OnlineTimeStamp
            };


            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CarOnlineStatuses.Update(carOnlineStatus);
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}