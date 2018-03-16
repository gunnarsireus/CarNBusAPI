using System.Threading.Tasks;
using Shared.Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models.Write;
using Shared.Models.Read;

namespace Server.CommandHandlers
{
    public class CreateCarOnlineStatusHandler : IHandleMessages<CreateCarOnlineStatus>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does Create mean?
        public CreateCarOnlineStatusHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<CreateCarOnlineStatusHandler>();

        public Task Handle(CreateCarOnlineStatus message, IMessageHandlerContext context)
        {
            log.Info("Received CreateCarOnlineStatus");

            var carOnlineStatus = new CarOnlineStatus
            {
                Online = message.OnlineStatus,
                CarId = message.CarId,
                OnlineTimeStamp = message.CreateCarOnlineTimeStamp
            };


            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CarOnlineStatuses.Add(carOnlineStatus);
                unitOfWork.CarsReadNull.Add(new CarReadNull(message.CarId, message.CompanyId)
                {
                    Online = message.OnlineStatus,
                    ChangeTimeStamp = message.CreateCarOnlineTimeStamp
                });
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}