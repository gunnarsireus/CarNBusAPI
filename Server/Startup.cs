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
            var serverFolder = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + Path.DirectorySeparatorChar + "Server" + Path.DirectorySeparatorChar;
            dbContextOptionsBuilder.UseSqlite("DataSource=" + serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "Car.db");
            services.AddSingleton(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<ApiContext>(options =>
                    options.UseSqlite("DataSource=" + serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "Car.db"));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<UpdateCarLockedStatusHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<UpdateCarOnlineStatusHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<DeleteCarHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<CreateCarHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<UpdateCompanyHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<DeleteCompanyHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);
            builder.RegisterType<CreateCompanyHandler>().AsSelf().WithParameter("dbContextOptionsBuilder", dbContextOptionsBuilder);

            Container = builder.Build();

            IEndpointInstance endpoint = null;
            builder.Register(c => endpoint)
                .As<IEndpointInstance>()
                .SingleInstance();

            var endpointConfiguration = new EndpointConfiguration("CarNBusAPI.Server");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.PurgeOnStartup(true);  //Only for demos!!

            //endpointConfiguration.Conventions().DefiningCommandsAs(t =>
            //        t.Namespace != null && t.Namespace.StartsWith("Messages") &&
            //        (t.Namespace.EndsWith("Commands")))
            //    .DefiningEventsAs(t =>
            //        t.Namespace != null && t.Namespace.StartsWith("Messages") &&
            //        t.Namespace.EndsWith("Events"));

            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingLifetimeScope(Container);
                });

            Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
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