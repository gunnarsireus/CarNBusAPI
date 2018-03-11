using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Server.DAL;
using Messages.Commands;
using NServiceBus;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using NServiceBus.Persistence.Sql;
using System.Data.SqlClient;
using System;

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
        Autofac.IContainer Container { get; set; }
        IConfigurationRoot Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var endpointConfiguration = new EndpointConfiguration("CarNBusAPI.Client");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var connection = "Server=tcp:sireusdbserver.database.windows.net,1433;Initial Catalog=dashdocssireus;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));

            transport.Routing().RouteToEndpoint(assembly: typeof(CreateCar).Assembly, destination: "CarNBusAPI.Server");
            transport.Routing().RouteToEndpoint(messageType: typeof(UpdateCarLockedStatus), destination: "CarNBusAPI.ServerPriority");
            endpointConfiguration.Conventions().DefiningCommandsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    (t.Namespace.EndsWith("Commands")))
                .DefiningEventsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    t.Namespace.EndsWith("Events"));

            EndpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            IEndpointInstance endpoint = null;
            containerBuilder.Register(c => endpoint)
                .As<IEndpointInstance>()
                .SingleInstance();

            Container = containerBuilder.Build();
            services.AddSingleton(EndpointInstance);
            services.AddSingleton(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);

            var serverFolder = Directory.GetParent(Directory.GetParent((Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString()).ToString() + Path.DirectorySeparatorChar;
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            dbContextOptionsBuilder.UseSqlite("DataSource=" + serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "Car.db");

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
