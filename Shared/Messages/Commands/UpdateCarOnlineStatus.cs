using System;
namespace Shared.Messages.Commands
{
    public class UpdateCarOnlineStatus 
	{
        public UpdateCarOnlineStatus()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public bool OnlineStatus { get; set; }
        public long UpdateCarOnlineTimeStamp { get; set; }
    }
}