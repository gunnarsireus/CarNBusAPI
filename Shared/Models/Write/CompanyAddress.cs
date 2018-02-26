using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Write
{
    public class CompanyAddress
	{
        public CompanyAddress()
        {
            AddressTimeStamp = DateTime.Now.Ticks;
        }
        [Key]
        public Guid CompanyId { get; set; }
        public long AddressTimeStamp { get; set; }
        public string Address { get; set; }
    }
}
