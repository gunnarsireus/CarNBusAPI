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
            var companyReadNull = new CompanyReadNull(message.CompanyId)
            {
                CreationTime = message.CreationTime,
                ChangeTimeStamp = message.CreateCompanyTimeStamp,
                Name = message.Name,
                Address = message.Address,
                Deleted = false
            };


            using (var unitOfWork = new CarUnitOfWork(new ApiContext(_dbContextOptionsBuilder.Options)))
            {
                unitOfWork.Companies.Add(company);
                unitOfWork.CompanyReadNulls.Add(companyReadNull);
                unitOfWork.Complete();
            }

            // publish an event that a company was created?
            return Task.CompletedTask;
        }
    }
}