using System;
using System.Globalization;

namespace CarNBusAPI.Models
{
	public class Car
	{
		public Car()
		{
			CreationTime = DateTime.Now.ToString(new CultureInfo("se-SE"));
			Online = true;
			Pending = "";
		}
		public Guid Id { get; set; }
		public Guid CompanyId { get; set; }
		public string CreationTime { get; set; }
		public string VIN { get; set; }
		public string RegNr { get; set; }
		public bool Online { get; set; }
		public bool Disabled { get; set; } //Used to block changes of Online/Offline status
		public string Pending { get; set; }  //Pending change sin database
	}
}
