using System.ComponentModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Server.DAL;
using Messages.Commands;
using NServiceBus;
using System.Threading.Tasks;
using System;
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
        IContainer ApplicationContainer { get; set; }
        IConfigurationRoot Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);

            var endpointConfiguration = new EndpointConfiguration("CarNBusAPI.Client");

            endpointConfiguration.UsePersistence<LearningPersistence>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UseTransport<LearningTransport>()
            .Routing().RouteToEndpoint(assembly: typeof(CreateCar).Assembly, destination: "CarNBusAPI.Server");


            endpointConfiguration.Conventions().DefiningCommandsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    (t.Namespace.EndsWith("Commands")))
                .DefiningEventsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    t.Namespace.EndsWith("Events"));

            EndpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            services.AddSingleton(EndpointInstance);

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarNBusAPI V1");
            });

            app.UseMvc();

            app.UseCors("AllowAllOrigins");

            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetService<ApiContext>();
            //    context.Database.EnsureCreated();
            //    context.EnsureSeedData();
            //}
        }
    }
}
