using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Shared.Models.Write
{
	public class Company
	{
        public Company()
        {
            CreationTime = DateTime.Now.ToString(new CultureInfo("sv-SE"));
        }
        public Company(Guid companyId) : this()
        {
            CompanyId = companyId;
        }
        [Key]
        public Guid CompanyId { get; set; }
		public string CreationTime { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
	}
}
