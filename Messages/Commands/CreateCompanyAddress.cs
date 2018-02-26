using System;

namespace Messages.Commands
{
    public class CreateCompanyAddress
    {
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public string Address { get; set; }
        public long CreateCompanyAddressTimeStamp { get; set; }
    }
}