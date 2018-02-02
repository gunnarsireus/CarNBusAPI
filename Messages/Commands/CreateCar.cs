using NServiceBus;
using System;
namespace Messages.Commands
{
    [Serializable]
    public class CreateCar : IMessage
    {
        public CreateCar()
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