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
    public class UpdateCompanyAddressHandler : IHandleMessages<UpdateCompanyAddress>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does update mean?
        public UpdateCompanyAddressHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<UpdateCompanyAddressHandler>();

        public Task Handle(UpdateCompanyAddress message, IMessageHandlerContext context)
        {

            log.Info("Received UpdateCompanyAddress");

            var companyAddress = new CompanyAddress
            {
                Address = message.Address,
                CompanyId = message.CompanyId,
                AddressTimeStamp = message.UpdateCompanyAddressTimeStamp
            };

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CompanyAddresses.Update(companyAddress);
                unitOfWork.CompanyReadNulls.Add(new CompanyReadNull(message.CompanyId)
                {
                    Address = message.Address,
                    ChangeTimeStamp = message.UpdateCompanyAddressTimeStamp
                });
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}