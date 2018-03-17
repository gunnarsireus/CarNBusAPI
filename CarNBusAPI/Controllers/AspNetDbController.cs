﻿using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Shared.Utils;
using Shared.Messages.Commands;
using System;
using NServiceBus;

namespace CarNBusAPI.Controllers
{
    [Route("api/[controller]")]
	public class AspNetDbController : Controller
	{
        readonly IEndpointInstance _endpointInstancePriority;
        public AspNetDbController(IEndpointInstance endpointInstancePriority, IConfigurationRoot configuration)
		{
			Configuration = configuration;
            _endpointInstancePriority = endpointInstancePriority;
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
        [HttpPost("/api/aspnetdb/clear")]
        [EnableCors("AllowAllOrigins")]
        public void ClearDatabase()
        {
            var message = new ClearDatabase
            {
                DataId = Guid.NewGuid()
            };

            _endpointInstancePriority.Send(message).ConfigureAwait(false);
        }

    }
}