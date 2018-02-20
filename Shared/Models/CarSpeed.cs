using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class CarSpeed
	{
        public CarSpeed()
        {
            SpeedTimeStamp = DateTime.Now.Ticks;
        }
        //[Key, ForeignKey("SpeedOf")]
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public long SpeedTimeStamp { get; set; }
        public int Speed { get; set; }
        //public Car SpeedOf { get; set; }
    }
}
