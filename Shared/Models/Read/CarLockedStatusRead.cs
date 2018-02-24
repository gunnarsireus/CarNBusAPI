using System;

namespace Shared.Models.Read
{
    public class CarLockedStatusRead
	{
        public CarLockedStatusRead()
        {
            Id = Guid.NewGuid();
            LockedTimeStamp = DateTime.Now.Ticks;
        }
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public bool Locked { get; set; }
        public long LockedTimeStamp { get; set; }
    }
}
