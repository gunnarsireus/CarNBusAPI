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
		public Guid CarId { get; set; }
		public string CreationTime { get; set; }
		public string VIN { get; set; }
		public string RegNr { get; set; }
        public Shared.Models.CarOnlineStatus _CarOnlineStatus { get; set; }
        public Shared.Models.CarLockedStatus _CarLockedStatus { get; set; }
        public Shared.Models.CarCompany _CarCompany { get; set; }
    }
}
