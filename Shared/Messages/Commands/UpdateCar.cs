using NServiceBus;
using System;
using Shared.Models.Write;
namespace Shared.Messages.Commands
{
    // What does update mean anyway?
    [Serializable]
    public class UpdateCar : IMessage
	{
        public UpdateCar()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CarId { get; set; }
		public string CreationTime { get; set; }
		public string VIN { get; set; }
		public string RegNr { get; set; }
        public CarOnlineStatus _CarOnlineStatus { get; set; }
        public CarLockedStatus _CarLockedStatus { get; set; }
    }
}
