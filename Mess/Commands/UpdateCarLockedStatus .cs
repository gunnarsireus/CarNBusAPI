using NServiceBus;
using System;
namespace Messages.Commands
{
    // What does update mean anyway?
    [Serializable]
    public class UpdateCarLockedStatus : IMessage
	{
        public UpdateCarLockedStatus()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CarId { get; set; }
        public bool LockedStatus { get; set; }
    }
}
