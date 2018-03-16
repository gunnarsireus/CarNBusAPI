using System;

namespace Shared.Messages.Commands
{
    public class UpdateCompanyAddress 
    {
        public UpdateCompanyAddress()
        {
            DataId = Guid.NewGuid();
        }
        public Guid DataId { get; set; }
        public Guid CompanyId { get; set; }
        public string Address { get; set; }
        public long UpdateCompanyAddressTimeStamp { get; set; }
    }
}
