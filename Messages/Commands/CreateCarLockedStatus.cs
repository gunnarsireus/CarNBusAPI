using System;

namespace Messages.Commands
{
    public class CreateCarLockedStatus
    {
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public bool LockedStatus { get; set; }
        public long CreationTime { get; set; }
    }
}