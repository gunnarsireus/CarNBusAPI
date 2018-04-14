using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Shared.Utils;

namespace CarNBusAPI
{
	public class Program
    {
	    internal static void Main(string[] args)
	    {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            var builder = new ConfigurationBuilder()
			    .SetBasePath(Directory.GetCurrentDirectory())
			    .AddJsonFile("appsettings.json");

		    builder.Build();
		    BuildWebHost(args).Run();
		}

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
	            .UseKestrel()
	            .UseContentRoot(Directory.GetCurrentDirectory())
				.UseStartup<Startup>()
                .Build();
    }
}
