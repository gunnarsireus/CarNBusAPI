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
    public class DeleteCarHandler : IHandleMessages<DeleteCar>
	{
		readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
		public DeleteCarHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
		}

		static ILog log = LogManager.GetLogger<DeleteCarHandler>();

		public Task Handle(DeleteCar message, IMessageHandlerContext context)
		{
			log.Info("Received DeleteCar.");

			using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
			{
				unitOfWork.Cars.Remove(new Car(message.CarId));
                unitOfWork.CarsReadNull.Add(new CarReadNull(message.CarId,message.CompanyId) {
                    Deleted = true,
                    ChangeTimeStamp = message.DeleteCarTimeStamp
                });
                unitOfWork.Complete();
			}

			// publish an event that a car had been deleted?
			return Task.CompletedTask;
		}
	}
}