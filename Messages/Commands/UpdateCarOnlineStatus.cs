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
		public Guid Id { get; set; }
		public Guid CompanyId { get; set; }
		public string CreationTime { get; set; }
		public string VIN { get; set; }
		public string RegNr { get; set; }
		public bool Online { get; set; }
		public bool Locked { get; set; } //Used to block changes of Online/Offline status
	}
}
