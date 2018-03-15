﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Messages.Commands;
using NServiceBus;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using NServiceBus.Persistence.Sql;
using System.Data.SqlClient;
using System;
using Shared.Utils;

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
        Autofac.IContainer Container { get; set; }
        IConfigurationRoot ConfigurationRoot { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var endpointConfiguration = Helpers.CreateEndpoint(Helpers.GetDbLocation(ConfigurationRoot["AppSettings:DbLocation"]), "carnbusapi-client");

            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>()
                            .ConnectionString(Helpers.GetStorageConnection());

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));

            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(Helpers.GetSqlConnection());
                });

            transport.Routing().RouteToEndpoint(assembly: typeof(CreateCar).Assembly, destination: "carnbusapi-server");
            transport.Routing().RouteToEndpoint(messageType: typeof(UpdateCarLockedStatus), destination: "carnbusapi-serverpriority");
            EndpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            IEndpointInstance endpoint = null;
            containerBuilder.Register(c => endpoint)
                .As<IEndpointInstance>()
                .SingleInstance();

            Container = containerBuilder.Build();
            services.AddSingleton(EndpointInstance);
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            app.UseMvc(route =>
            {
                route.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseCors("AllowAllOrigins");
        }
    }
}
