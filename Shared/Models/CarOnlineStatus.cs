using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class CarOnlineStatus
	{
        public CarOnlineStatus()
        {
            OnlineTimeStamp = DateTime.Now.Ticks;
        }
        //[Key, ForeignKey("OnlineStatusOf")]
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public long OnlineTimeStamp { get; set; }
        public bool Online { get; set; }
        //public Car OnlineStatusOf { get; set; }
	}
}
