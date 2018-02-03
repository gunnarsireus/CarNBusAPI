using NServiceBus;
using System;
namespace Messages.Commands
{
    // What does update mean anyway?
    [Serializable]
    public class UpdateCarOnlineStatus : IMessage
	{
        public UpdateCarOnlineStatus()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CarId { get; set; }
        public bool OnlineStatus { get; set; }
    }
}
