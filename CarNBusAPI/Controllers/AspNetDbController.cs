using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;

namespace CarNBusAPI.Controllers
{
    [Route("api/[controller]")]
	public class AspNetDbController : Controller
	{
		public AspNetDbController(IConfigurationRoot configuration)
		{
			Configuration = configuration;
		}
		IConfigurationRoot Configuration { get; set; }
		// GET api/Car
		[HttpGet]
		[EnableCors("AllowAllOrigins")]
		public string GetAspNetDb()
		{
            var serverFolder = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + Path.DirectorySeparatorChar + "Server" + Path.DirectorySeparatorChar;
            return serverFolder + Configuration["AppSettings:DbLocation"] + Path.DirectorySeparatorChar + "AspNet.db";
		}
	}
}