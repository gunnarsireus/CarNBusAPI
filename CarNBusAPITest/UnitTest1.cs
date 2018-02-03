using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CarNBusAPI.Controllers;
using Server.DAL;
using Shared.Models;
using Xunit;
using NServiceBus;
using Microsoft.Extensions.Configuration;

namespace CarNBusAPITest
{
	public class UnitTest1
	{
        readonly IEndpointInstance _endpointInstance;
        IConfigurationRoot Configuration { get; set; }
        [Fact(DisplayName = "Create a company and a vehicle")]
		public void Test1()
		{
			// In-memory database only exists while the connection is open
			var connection = new SqliteConnection("DataSource=:memory:");
			connection.Open();
			try
			{
				var options = new DbContextOptionsBuilder<ApiContext>()
					.UseSqlite(connection)
					.Options;

				// Create the schema in the database
				using (var context = new ApiContext(options))
				{
					context.Database.EnsureCreated();
				}
				using (var context = new ApiContext(options))
				{
					var companyId = Guid.NewGuid();
					var company = new Company { Id = companyId };
					context.Companies.Add(company);

					var car = new Car { _CarCompany = new CarCompany { CompanyId = company.Id }, VIN = "xxx", RegNr = "ABC123" };
					context.Cars.Add(car);

					context.SaveChanges();
				}

                using (var context = new ApiContext(options))
                {
                    var bc = new CarController(_endpointInstance, Configuration);
                    var result = bc.GetCars();
                    Assert.NotNull(result);
                    Assert.Equal(result.Count(), 1);
                }

            }
			finally
			{
				connection.Close();
			}
		}
	}
}
