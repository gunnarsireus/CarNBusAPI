using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class CarSpeed
	{
        [Key, ForeignKey("SpeedOf")]
        public Guid CarId { get; set; }
        public int Speed { get; set; }
        public Car SpeedOf { get; set; }
    }
}
