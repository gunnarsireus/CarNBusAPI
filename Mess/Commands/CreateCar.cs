using NServiceBus;
using System;
using Shared.Models;
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
		public Guid CarId { get; set; }
		public string CreationTime { get; set; }
		public string VIN { get; set; }
		public string RegNr { get; set; }
        public CarOnlineStatus _CarOnlineStatus { get; set; }
        public CarLockedStatus _CarLockedStatus { get; set; }
        public Guid CompanyId { get; set; }
    }
}
