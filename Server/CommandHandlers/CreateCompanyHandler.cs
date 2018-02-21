using System.Threading.Tasks;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models.Write;

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
            log.Info("Received CreateCompany");

            var company = new Company
            {
                Address = message.Address,
                CreationTime = message.CreationTime,
                CompanyId = message.CompanyId,
                Name = message.Name
            };


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