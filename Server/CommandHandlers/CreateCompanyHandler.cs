using System.Threading.Tasks;
using Shared.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models;

namespace Server.CommandHandlers
{
    public class CreateCompanyHandler : IHandleMessages<CreateCompany>
	{
		readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
		public CreateCompanyHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
		}

		static ILog log = LogManager.GetLogger<CreateCompany>();

		public Task Handle(CreateCompany message, IMessageHandlerContext context)
		{
			log.Info("Received CreateCompanyRequest");

			var company = new Company(message.Id);
			// TODO: map object and massege

			using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
			{
				unitOfWork.Companies.Add(company);
				unitOfWork.Complete();
			}

			// publish an event that a company was created?
			return Task.CompletedTask;
		}
	}
}