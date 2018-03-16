using System;

namespace Shared.Messages.Commands
{
    public class CreateCarOnlineStatus
    {
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public bool OnlineStatus { get; set; }
        public long CreateCarOnlineTimeStamp { get; set; }
    }
}