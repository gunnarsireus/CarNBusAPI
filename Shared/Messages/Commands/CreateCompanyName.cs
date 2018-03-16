using System;

namespace Shared.Messages.Commands
{
    public class CreateCompanyName
    {
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public long CreateCompanyNameTimeStamp { get; set; }
    }
}