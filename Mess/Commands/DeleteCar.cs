using NServiceBus;
using System;
namespace Messages.Commands
{
    [Serializable]
    public class DeleteCar : IMessage
    {
        public DeleteCar()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CarId { get; set; }
	}
}
