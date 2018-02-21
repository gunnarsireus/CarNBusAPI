using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Write
{
    public class CarSpeed
	{
        public CarSpeed()
        {
            SpeedTimeStamp = DateTime.Now.Ticks;
        }
        [Key]
        public Guid CarId { get; set; }
        public long SpeedTimeStamp { get; set; }
        public int Speed { get; set; }
    }
}
