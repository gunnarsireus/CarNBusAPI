using System;

namespace Shared.Models.Read
{
    public class CompanyAddressRead
	{
        public CompanyAddressRead()
        {
            AddressTimeStamp = DateTime.Now.Ticks;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public long AddressTimeStamp { get; set; }
        public string Address { get; set; }
    }
}
