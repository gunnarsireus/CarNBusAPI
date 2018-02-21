using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Write
{
    public class CarLockedStatus
	{
        public CarLockedStatus()
        {
            LockedTimeStamp = DateTime.Now.Ticks;
        }
        [Key]
        public Guid CarId { get; set; }
        public bool Locked { get; set; }
        public long LockedTimeStamp { get; set; }
    }
}
