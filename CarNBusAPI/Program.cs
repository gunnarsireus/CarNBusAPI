using System;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CarNBusAPI
{
	public class Program
    {
	    public static void Main(string[] args)
	    {
		    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            FileStream filestream = new FileStream("out.txt", FileMode.Create);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            Console.WriteLine("Directory.GetCurrentDirectory(): " + Directory.GetCurrentDirectory());
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
