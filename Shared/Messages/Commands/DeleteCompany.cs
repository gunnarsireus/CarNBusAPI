using System;
namespace Shared.Messages.Commands
{
    public class DeleteCompany 
    {
        public DeleteCompany()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CompanyId { get; set; }
        public long DeleteCompanyTimeStamp { get; set; }
    }
}
