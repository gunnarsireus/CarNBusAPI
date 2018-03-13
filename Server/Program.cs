using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Microsoft.AspNetCore;
using System.IO;
using System;

namespace Server
{

	public class Program
	{
		public static void Main(string[] args)
		{
			CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            BuildWebHost(args).Run();
		}

		static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();
	}

}
