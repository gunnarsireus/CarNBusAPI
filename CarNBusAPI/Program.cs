using System.Globalization;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CarNBusAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
	        CultureInfo.CurrentUICulture = new CultureInfo("en-US");
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
