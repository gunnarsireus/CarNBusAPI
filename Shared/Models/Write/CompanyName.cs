using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Write
{
    public class CompanyName
	{
        public CompanyName()
        {
            NameTimeStamp = DateTime.Now.Ticks;
        }
        [Key]
        public Guid CompanyId { get; set; }
        public long NameTimeStamp { get; set; }
        public string Name { get; set; }
    }
}
