using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Write
{
    public class CarOnlineStatus
	{
        public CarOnlineStatus()
        {
            OnlineTimeStamp = DateTime.Now.Ticks;
        }
        [Key]
        public Guid CarId { get; set; }
        public long OnlineTimeStamp { get; set; }
        public bool Online { get; set; }
	}
}
