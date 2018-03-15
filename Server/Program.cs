using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Microsoft.AspNetCore;
using System.IO;
using System;
using Shared.Utils;

namespace Server
{

	public class Program
	{
   		internal static void Main(string[] args)
		{
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            Helpers.RedirectConsoleToTextFile("out2.txt");
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
