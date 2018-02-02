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

	public class UpdateCarHandler : IHandleMessages<UpdateCar>
	{
		readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
		// What does update mean?
		public UpdateCarHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
		}

		static ILog log = LogManager.GetLogger<UpdateCarHandler>();

		public Task Handle(UpdateCar message, IMessageHandlerContext context)
		{
			log.Info("Received UpdateCar.");

			var car = new Car();
			// TODO: map object and massege

			car.Id = message.Id;

			using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
			{
				unitOfWork.Cars.Update(car);
				unitOfWork.Complete();
			}

			return Task.CompletedTask;
		}
	}
}