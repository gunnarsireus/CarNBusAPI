﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using NServiceBus;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Shared.Utils;
using System;
using Microsoft.Extensions.Logging;
using System.IO;

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
            Configuration = builder.Build();
        }

        IEndpointInstance EndpointInstance { get; set; }
        IEndpointInstance EndpointInstancePriority { get; set; }
        Autofac.IContainer Container { get; set; }
        IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var endpointConfiguration = Helpers.CreateEndpoint(Helpers.ApiEndpoint, Directory.GetCurrentDirectory() + "\\App_Data");
            EndpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var endpointConfigurationPriority = Helpers.CreatePriorityEndpointPublisher(Directory.GetCurrentDirectory() + "\\App_Data");
            EndpointInstancePriority = Endpoint.Start(endpointConfigurationPriority).GetAwaiter().GetResult();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            Container = containerBuilder.Build();
            services.AddSingleton(EndpointInstance);
            services.AddSingleton(EndpointInstancePriority);
            services.AddSingleton(Configuration);
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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
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
