using NServiceBus;
using System;
namespace Messages.Commands
{
    // What does update mean anyway?
    [Serializable]
    public class UpdateCarSpeed : IMessage
    {
        public UpdateCarSpeed()
        {
            DataId = Guid.NewGuid();
        }
        public Guid DataId { get; set; }
        public Guid CarId { get; set; }
        public long Speed { get; set; }
    }
}
