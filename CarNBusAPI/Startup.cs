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
using NServiceBus.Features;

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
            var endpointConfiguration = new EndpointConfiguration("carnbusapi-client");
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.DisableFeature<TimeoutManager>();
            endpointConfiguration.DisableFeature<MessageDrivenSubscriptions>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            var connection = "Server=tcp:sireusdbserver.database.windows.net,1433;Initial Catalog=dashdocssireus;Persist Security Info=False;User ID=sireus;Password=GS1@azure;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var storageConnection = @"DefaultEndpointsProtocol=https;AccountName=carnbusstorage;AccountKey=u6UlmCvk4muPIStmGWmLmYXwk9LQdX+HECgrSQxg0AkDZB4IBs2kUu9z6Ih4LlyU4Ren9VtVWKT232cyahex8Q==";

            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>()
                            .ConnectionString(storageConnection);

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));

            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            transport.Routing().RouteToEndpoint(assembly: typeof(CreateCar).Assembly, destination: "carnbusapi-server");
            transport.Routing().RouteToEndpoint(messageType: typeof(UpdateCarLockedStatus), destination: "carnbusapi-serverpriority");
            endpointConfiguration.Conventions().DefiningMessagesAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    (t.Namespace.EndsWith("Commands")))
                .DefiningEventsAs(t =>
                    t.Namespace != null && t.Namespace.StartsWith("Messages") &&
                    t.Namespace.EndsWith("Events"));

            endpointConfiguration.PurgeOnStartup(true);
            var serverFolder = Directory.GetParent(Directory.GetParent((Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString()).ToString() + Path.DirectorySeparatorChar;
            var dbLocation = serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar;
            if (!Directory.Exists(dbLocation)) { Directory.CreateDirectory(dbLocation); }
            endpointConfiguration.LicensePath(dbLocation + "License.xml");
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


            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApiContext>();

            dbContextOptionsBuilder.UseSqlite("DataSource=" + dbLocation + "Car.db");

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
