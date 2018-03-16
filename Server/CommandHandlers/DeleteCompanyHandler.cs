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
            log.Info("Received DeleteCompany");
            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                // Send delete all cars command for company
                unitOfWork.Companies.Remove(new Company(message.CompanyId));
                unitOfWork.CompanyReadNulls.Add(new CompanyReadNull(message.CompanyId)
                {
                    Deleted = true,
                    ChangeTimeStamp = message.DeleteCompanyTimeStamp
                });
                unitOfWork.Complete();
            }

            // publish an event that a company had been deleted?
            return Task.CompletedTask;
        }
    }
}