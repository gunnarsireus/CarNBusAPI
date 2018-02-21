using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Read
{
    public class CompanyRead
    {
        [Key]
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pending { get; set; }  
    }
}
