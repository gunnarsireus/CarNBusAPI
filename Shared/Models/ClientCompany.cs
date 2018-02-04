using System;

namespace Shared.Models
{
    public class ClientCompany
    {
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pending { get; set; }  
    }
}
