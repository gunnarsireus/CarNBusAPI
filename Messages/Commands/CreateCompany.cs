using System;
using System.Collections.Generic;
namespace Messages.Commands
{
    public class CreateCompany
    {
        public CreateCompany()
        {
            DataId = Guid.NewGuid();
        }
        public Guid DataId { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long CreateCompanyTimeStamp { get; set; }
    }
}
