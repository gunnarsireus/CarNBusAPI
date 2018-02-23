using System;
namespace Messages.Commands
{
    public class DeleteCompany 
    {
        public DeleteCompany()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CompanyId { get; set; }
        public long DeleteTimeStamp { get; set; }
    }
}
