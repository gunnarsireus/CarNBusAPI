using System;
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
			return Directory.GetCurrentDirectory() + Configuration["AppSettings:DbLocation"] + @"\AspNet.db";
		}
	}
}