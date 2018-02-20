using System.Threading.Tasks;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using Server.Data;
using Server.DAL;
using Shared.Models;

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
                var allCars = unitOfWork.Cars.GetAllByCompanyId(message.CompanyId);
                foreach (var car in allCars)
                {
                    unitOfWork.CarLockedStatus.Add(new CarLockedStatus { Locked = true, CarId = car.CarId });
                    unitOfWork.Complete();
                }
                unitOfWork.Cars.RemoveRange(allCars);
                unitOfWork.Complete();
                unitOfWork.Companies.Remove(unitOfWork.Companies.Get(message.CompanyId));
                unitOfWork.Complete();
            }

            // publish an event that a company had been deleted?
            return Task.CompletedTask;
        }
    }
}