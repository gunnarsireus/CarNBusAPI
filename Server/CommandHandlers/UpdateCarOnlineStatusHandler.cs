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
			log.Info("Received UpdateCar.");

			var car = new CarOnlineStatus();
			car.Online = message.Online;
			// TODO: map object and massege

			using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
			{
				// TODO: fix the unit of work
				// unitOfWork.Cars.Update(car);
				unitOfWork.Complete();
			}

			return Task.CompletedTask;
		}
	}
}