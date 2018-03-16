using System.Threading.Tasks;
using Shared.Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models.Write;
using Shared.Models.Read;
using System;
using System.Globalization;

namespace Server.CommandHandlers
{
    public class CreateCompanyAddressHandler : IHandleMessages<CreateCompanyAddress>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does Create mean?
        public CreateCompanyAddressHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<CreateCompanyAddressHandler>();

        public Task Handle(CreateCompanyAddress message, IMessageHandlerContext context)
        {

            log.Info("Received CreateCompanyAddress");

            var companyAddress = new CompanyAddress
            {
                Address = message.Address,
                CompanyId = message.CompanyId,
                AddressTimeStamp = message.CreateCompanyAddressTimeStamp
            };

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CompanyAddresses.Add(companyAddress);
                unitOfWork.CompanyReadNulls.Add(new CompanyReadNull(message.CompanyId)
                {
                    Address = message.Address,
                    ChangeTimeStamp = message.CreateCompanyAddressTimeStamp
                });
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}