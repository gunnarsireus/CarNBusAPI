namespace Server.Requesthandler
{
	using System.Threading.Tasks;
	using Shared.Commands;
	using Microsoft.EntityFrameworkCore;
	using NServiceBus;
	using NServiceBus.Logging;
	using Server.Data;
	using Server.DAL;
	using Shared.Models;

	public class UpdateCompanyHandler : IHandleMessages<UpdateCompany>
	{
		readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
		public UpdateCompanyHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
		}
		static ILog log = LogManager.GetLogger<UpdateCompany>();

		public Task Handle(UpdateCompany message, IMessageHandlerContext context)
		{
			log.Info("Received UpdateCompanyRequest");

			var company = new Company(message.Id);
			// TODO: map object and massege

			using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
			{
				unitOfWork.Companies.Update(company);
				unitOfWork.Complete();
			}

			return Task.CompletedTask;

		}
	}
}