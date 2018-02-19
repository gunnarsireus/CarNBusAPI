using System;
namespace Messages.Commands
{
    public class UpdateCarLockedStatus
    {
        public UpdateCarLockedStatus()
        {
            DataId = Guid.NewGuid();
        }
        public Guid DataId { get; set; }
        public Guid CarId { get; set; }
        public long LockedTimeStamp { get; set; }
        public bool LockedStatus { get; set; }
    }
}
