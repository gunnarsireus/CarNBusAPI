using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Shared.Utils;
using Shared.Messages.Commands;
using System;
using NServiceBus;
using System.Threading.Tasks;
using Shared.Messages.Events;

namespace CarNBusCarNBusAPI.Controllers
{
    [Route("api/[controller]")]
	public class AspNetDbController : Controller
	{
        readonly IEndpointInstance _endpointInstancePriority;
        public AspNetDbController(IEndpointInstance endpointInstancePriority, IConfiguration configuration)
		{
			Configuration = configuration;
            _endpointInstancePriority = endpointInstancePriority;
        }
		IConfiguration Configuration { get; set; }
		// GET api/Car
		[HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [EnableCors("AllowAllOrigins")]
		public string GetAspNetDb()
		{
            //return Helpers.GetDbLocation(Configuration["AppSettings:DbLocation"]) + "AspNet.db";
            return Helpers.GetAspNetDbConnection();
        }

        [HttpPost("/api/aspnetdb/clear")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [EnableCors("AllowAllOrigins")]
        public async Task ClearDatabase()
        {
            var message = new ClearDatabase
            {
                DataId = Guid.NewGuid()
            };

            await _endpointInstancePriority.Publish(message).ConfigureAwait(false);
        }

    }
}