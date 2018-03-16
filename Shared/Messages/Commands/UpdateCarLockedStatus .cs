using System;
namespace Shared.Messages.Commands
{
    public class UpdateCarLockedStatus
    {
        public UpdateCarLockedStatus()
        {
            DataId = Guid.NewGuid();
        }
        public Guid DataId { get; set; }
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public long UpdateCarLockedTimeStamp { get; set; }
        public bool LockedStatus { get; set; }
    }
}
