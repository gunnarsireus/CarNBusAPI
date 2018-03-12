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
            Configuration = builder.Build();
        }

        IContainer Container { get; set; }
        IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            var serverFolder = Directory.GetParent(Directory.GetParent((Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString()).ToString() + Path.DirectorySeparatorChar;
            dbContextOptionsBuilder.UseSqlite("DataSource=" + serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "Car.db");
            services.AddSingleton(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<ApiContext>(options =>
                    options.UseSqlite("DataSource=" + serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "Car.db"));

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
            builder.RegisterType<UpdateCompanyHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);

            Container = builder.Build();

            IEndpointInstance endpoint = null;
            builder.Register(c => endpoint)
                .As<IEndpointInstance>()
                .SingleInstance();

            var endpointConfiguration = new EndpointConfiguration("carnbusapi-server");
            endpointConfiguration.DisableFeature<TimeoutManager>();
            endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));

            var connection = "Server=tcp:sireusdbserver.database.windows.net,1433;Initial Catalog=dashdocssireus;Persist Security Info=False;User ID=sireus;Password=GS1@azure;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var storageConnection = @"DefaultEndpointsProtocol=https;AccountName=carnbusstorage;AccountKey=XGoQFAa/nGH7/lCC2NuEL2X4OLZWzCDS4+h8iAb0AFKmk+g3zXfkdHT/1lV0nWLVHbQkVfeZGl6mWTMKm9LMQg==;EndpointSuffix=core.windows.net";

            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>()
                                        .ConnectionString(storageConnection);

            endpointConfiguration.Conventions().DefiningCommandsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    (t.Namespace.EndsWith("Commands")))
                .DefiningEventsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    t.Namespace.EndsWith("Events"));

            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingLifetimeScope(Container);
                });

            var endpointConfigurationPriority = new EndpointConfiguration("carnbusapi-serverpriority");
            endpointConfigurationPriority.DisableFeature<TimeoutManager>();
            endpointConfigurationPriority.DisableFeature<MessageDrivenSubscriptions>();
            endpointConfigurationPriority.UseSerialization<NewtonsoftSerializer>();
            endpointConfigurationPriority.EnableInstallers();
            endpointConfigurationPriority.SendFailedMessagesTo("error");
            var persistencePriority = endpointConfigurationPriority.UsePersistence<SqlPersistence>();
            var subscriptionsPriority = persistencePriority.SubscriptionSettings();
            subscriptionsPriority.CacheFor(TimeSpan.FromMinutes(1));

            persistencePriority.SqlDialect<SqlDialect.MsSqlServer>();
            persistencePriority.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            var transportPriority = endpointConfigurationPriority.UseTransport<AzureStorageQueueTransport>()
                                            .ConnectionString(storageConnection);

            endpointConfigurationPriority.Conventions().DefiningCommandsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    (t.Namespace.EndsWith("Commands")))
                .DefiningEventsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    t.Namespace.EndsWith("Events"));

            endpointConfigurationPriority.UseContainer<AutofacBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingLifetimeScope(Container);
                });
            Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            Endpoint.Start(endpointConfigurationPriority).GetAwaiter().GetResult();
            return new AutofacServiceProvider(Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
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