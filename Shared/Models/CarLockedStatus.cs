using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class CarLockedStatus
	{
        public CarLockedStatus()
        {
            LockedTimeStamp = DateTime.Now.Ticks;
        }
        //[Key, ForeignKey("LockedStatusOf")]
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public bool Locked { get; set; }
        public long LockedTimeStamp { get; set; }
        //public Car LockedStatusOf { get; set; }
    }
}
