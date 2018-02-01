namespace Server.CommandHandlers
{
	using System.Threading.Tasks;
	using Shared.Commands;
	using Microsoft.EntityFrameworkCore;
	using NServiceBus;
	using NServiceBus.Logging;
	using Server.Data;
	using Server.DAL;
	using Shared.Models;

	public class CreateCarHandler : IHandleMessages<CreateCar>
	{
		readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
		public CreateCarHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
		}

		static ILog log = LogManager.GetLogger<CreateCarHandler>();

		public Task Handle(CreateCar message, IMessageHandlerContext context)
		{
			log.Info("Received CreateCar.");

			var car = new Car();
			car.Id = message.Id;
			// TODO: map object and massege

			using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
			{
				unitOfWork.Cars.Add(car);
				unitOfWork.Complete();
			}

			// Publish an event that a car was created?
			return Task.CompletedTask;
		}
	}
}