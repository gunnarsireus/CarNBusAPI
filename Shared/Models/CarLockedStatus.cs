using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class CarLockedStatus
	{
        [Key, ForeignKey("LockedStatusOf")]
        public Guid CarId { get; set; }
        public bool Locked { get; set; }
        public long LockedTimeStamp { get; set; }
        public Car LockedStatusOf { get; set; }
    }
}
