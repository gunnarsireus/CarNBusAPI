using System;

namespace Shared.Models.Read
{
    public class CompanyNameRead
	{
        public CompanyNameRead()
        {
            NameTimeStamp = DateTime.Now.Ticks;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public long NameTimeStamp { get; set; }
        public string Name { get; set; }
    }
}
