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
    public class UpdateCompanyNameHandler : IHandleMessages<UpdateCompanyName>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does update mean?
        public UpdateCompanyNameHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<UpdateCompanyNameHandler>();

        public Task Handle(UpdateCompanyName message, IMessageHandlerContext context)
        {

            log.Info("Received UpdateCompanyName");

            var companyName = new CompanyName
            {
                Name = message.Name,
                CompanyId = message.CompanyId,
                NameTimeStamp = message.UpdateCompanyNameTimeStamp
            };

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CompanyNames.Update(companyName);
                unitOfWork.CompanyReadNulls.Add(new CompanyReadNull(message.CompanyId)
                {
                    Name = message.Name,
                    ChangeTimeStamp = message.UpdateCompanyNameTimeStamp
                });
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}