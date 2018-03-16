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
    public class CreateCompanyNameHandler : IHandleMessages<CreateCompanyName>
    {
        readonly DbContextOptionsBuilder<ApiContext> _dbContextOptionsBuilder;
        // What does Create mean?
        public CreateCompanyNameHandler(DbContextOptionsBuilder<ApiContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        static ILog log = LogManager.GetLogger<CreateCarSpeedHandler>();

        public Task Handle(CreateCompanyName message, IMessageHandlerContext context)
        {

            log.Info("Received CreateCompanyName");

            var companyName = new CompanyName
            {
                Name = message.Name,
                CompanyId = message.CompanyId,
                NameTimeStamp = message.CreateCompanyNameTimeStamp
            };

            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.CompanyNames.Add(companyName);
                unitOfWork.CompanyReadNulls.Add(new CompanyReadNull(message.CompanyId)
                {
                    Name = message.Name,
                    ChangeTimeStamp = message.CreateCompanyNameTimeStamp
                });
                unitOfWork.Complete();
            }

            return Task.CompletedTask;
        }
    }
}