using System;

namespace Shared.Models.Read
{
    public class CarOnlineStatusRead
	{
        public CarOnlineStatusRead()
        {
            OnlineTimeStamp = DateTime.Now.Ticks;
        }
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public long OnlineTimeStamp { get; set; }
        public bool Online { get; set; }
	}
}
