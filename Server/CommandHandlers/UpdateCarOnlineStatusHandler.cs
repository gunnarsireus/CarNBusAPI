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
                CarId = message.CarId
            };


            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
			{
                var car = unitOfWork.Cars.Get(message.CarId);
                if (car == null) return Task.CompletedTask;
                var listOfOldItems = unitOfWork.CarOnlineStatus.GetAllOrdered(message.CarId);
                unitOfWork.CarOnlineStatus.Add(carOnlineStatus);
                unitOfWork.Complete();
                unitOfWork.CarOnlineStatus.RemoveRange(listOfOldItems);
                unitOfWork.Complete();
			}

			return Task.CompletedTask;
		}
	}
}