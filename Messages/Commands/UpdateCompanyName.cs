using System;

namespace Messages.Commands
{
    public class UpdateCompanyName 
    {
        public UpdateCompanyName()
        {
            DataId = Guid.NewGuid();
        }
        public Guid DataId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public long UpdateCompanyNameTimeStamp { get; set; }
    }
}
