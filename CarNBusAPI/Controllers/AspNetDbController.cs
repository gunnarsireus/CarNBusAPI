using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Shared.Utils;

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
            //return Helpers.GetDbLocation(Configuration["AppSettings:DbLocation"]) + "AspNet.db";
            return Helpers.GetAspNetDbConnection();

        }
	}
}