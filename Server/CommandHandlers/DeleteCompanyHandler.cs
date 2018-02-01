namespace Server.Requesthandler
{
	using System.Threading.Tasks;
	using Shared.Commands;
	using Microsoft.EntityFrameworkCore;
	using NServiceBus;
	using NServiceBus.Logging;
	using Server.Data;
	using Server.DAL;

    public class DeleteCompanyHandler : IHandleMessages<DeleteCompany>
	{
		readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
		public DeleteCompanyHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
		}

		static ILog log = LogManager.GetLogger<DeleteCompanyHandler>();

		public Task Handle(DeleteCompany message, IMessageHandlerContext context)
		{
			log.Info("Received DeleteCompanyRequest");
			using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
			{
				unitOfWork.Companies.Remove(unitOfWork.Companies.Get(message.CompanyId));
				unitOfWork.Complete();
			}

			// publish an event that a company had been deleted?
			return Task.CompletedTask;
		}
	}
}