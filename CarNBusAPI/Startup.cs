using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Shared.Messages.Commands;
using NServiceBus;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Shared.Utils;
using Shared.Messages.Events;
using System;
using Microsoft.Extensions.Logging;

namespace CarNBusAPI
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

        IEndpointInstance EndpointInstance { get; set; }
        IEndpointInstance EndpointInstancePriority { get; set; }
        Autofac.IContainer Container { get; set; }
        IConfigurationRoot ConfigurationRoot { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var endpointConfiguration = Helpers.CreateEndpoint(Helpers.GetDbLocation(ConfigurationRoot["AppSettings:DbLocation"]), "carnbusapi-client");
            endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                           .ConnectionString(Helpers.GetStorageConnection());

            endpointConfiguration.UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()
           .ConnectionString(Helpers.GetStorageConnection())
           .CreateSchema(true)
           .TimeoutManagerDataTableName("TimeoutManager")
           .TimeoutDataTableName("TimeoutData")
           .CatchUpInterval(3600)
           .PartitionKeyScope("2018052400");

            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>()
                                        .ConnectionString(Helpers.GetStorageConnection());

            transport.Routing().RouteToEndpoint(assembly: typeof(CreateCar).Assembly, @namespace: "Shared.Messages.Commands", destination: "carnbusapi-server");
            EndpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var endpointConfigurationPriority = Helpers.CreateEndpoint(Helpers.GetDbLocation(ConfigurationRoot["AppSettings:DbLocation"]), "carnbusapi-clientPriority");
            endpointConfigurationPriority.UsePersistence<AzureStoragePersistence, StorageType.Subscriptions>()
                           .ConnectionString(Helpers.GetStorageConnection());

            endpointConfigurationPriority.UsePersistence<AzureStoragePersistence, StorageType.Timeouts>()
           .ConnectionString(Helpers.GetStorageConnection())
           .CreateSchema(true)
           .TimeoutManagerDataTableName("TimeoutManagerPriority")
           .TimeoutDataTableName("TimeoutDataPriority")
           .CatchUpInterval(3600)
           .PartitionKeyScope("2018052400");

            var transportPriority = endpointConfigurationPriority.UseTransport<AzureStorageQueueTransport>()
                                        .ConnectionString(Helpers.GetStorageConnection());

            transportPriority.Routing().RouteToEndpoint(assembly: typeof(UpdateCarLockedStatus).Assembly,@namespace: "Shared.Messages.Events", destination: "carnbusapi-serverpriority");
            EndpointInstancePriority = Endpoint.Start(endpointConfigurationPriority).GetAwaiter().GetResult();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            Container = containerBuilder.Build();
            services.AddSingleton(EndpointInstance);
            services.AddSingleton(EndpointInstancePriority);
            services.AddSingleton(ConfigurationRoot);
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CarNBusAPI", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            Console.WriteLine("Configure...");
            loggerFactory.AddConsole(ConfigurationRoot.GetSection("Logging"));
            loggerFactory.AddDebug();
            appLifetime.ApplicationStopped.Register(() => Container.Dispose());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarNBusApi V1");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();

            app.UseCors("AllowAllOrigins");
        }
    }
}
