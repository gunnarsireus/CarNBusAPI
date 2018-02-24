using System;

namespace Shared.Models.Read
{
    public class CarSpeedRead
	{
        public CarSpeedRead()
        {
            SpeedTimeStamp = DateTime.Now.Ticks;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public long SpeedTimeStamp { get; set; }
        public int Speed { get; set; }
    }
}
