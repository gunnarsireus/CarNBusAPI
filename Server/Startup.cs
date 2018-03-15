using System;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Server.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using Server.CommandHandlers;
using NServiceBus.Persistence.Sql;
using System.Data.SqlClient;
using NServiceBus.Features;
using Microsoft.WindowsAzure.Storage;
using Shared.Utils;

namespace Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            ConfigurationRoot = builder.Build();
        }

        IContainer Container { get; set; }
        IConfigurationRoot ConfigurationRoot { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            var dbLocation = Helpers.GetDbLocation(ConfigurationRoot["AppSettings:DbLocation"]);
            Console.WriteLine("Server dbLocation: " + dbLocation);
            var dataSource = "DataSource=" + dbLocation + "Car.db";
            dbContextOptionsBuilder.UseSqlite(dataSource);
            services.AddSingleton(ConfigurationRoot);

            services.AddDbContext<ApiContext>(options =>
                    options.UseSqlite(dataSource));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<CreateCarHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<CreateCarLockedStatusHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<CreateCarOnlineStatusHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<CreateCarSpeedHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<CreateCompanyHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<CreateCompanyNameHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<CreateCompanyAddressHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<DeleteCarHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<DeleteCompanyHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<UpdateCarLockedStatusHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<UpdateCarOnlineStatusHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<UpdateCarSpeedHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<UpdateCompanyAddressHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<UpdateCompanyNameHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            Container = builder.Build();

            IEndpointInstance endpoint = null;
            builder.Register(c => endpoint)
                .As<IEndpointInstance>()
                .SingleInstance();

            var endpointConfiguration = Helpers.CreateEndpoint(Helpers.GetDbLocation(ConfigurationRoot["AppSettings:DbLocation"]), "carnbusapi-server");

            Helpers.CreatePersistenceAndTransport(out TransportExtensions<AzureStorageQueueTransport> transport, endpointConfiguration);

            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingLifetimeScope(Container);
                });

            var endpointConfigurationPriority = Helpers.CreateEndpoint(Helpers.GetDbLocation(ConfigurationRoot["AppSettings:DbLocation"]), "carnbusapi-serverpriority");

            Helpers.CreatePersistenceAndTransport(out TransportExtensions<AzureStorageQueueTransport> transportPriority, endpointConfigurationPriority);

            endpointConfigurationPriority.UseContainer<AutofacBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingLifetimeScope(Container);
                });
            Endpoint.Start(endpointConfiguration);
            Endpoint.Start(endpointConfigurationPriority);
            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(ConfigurationRoot.GetSection("Logging"));
            loggerFactory.AddDebug();
            appLifetime.ApplicationStopped.Register(() => Container.Dispose());
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApiContext>();
                context.Database.EnsureCreated();
                context.EnsureSeedData();
            }
        }
    }
}