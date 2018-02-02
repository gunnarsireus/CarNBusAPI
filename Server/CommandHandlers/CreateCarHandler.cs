using System.Threading.Tasks;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.DAL;
using Server.Data;
using Shared.Models;

namespace Server.CommandHandlers
{
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
			log.Info("Received CreateCar");

            var car = new Car
            {
                CompanyId = message.CompanyId,
                CreationTime = message.CreationTime,
                Id = message.Id,
                Locked = message.Locked,
                Online = message.Online,
                RegNr = message.RegNr,
                VIN = message.VIN
            };
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