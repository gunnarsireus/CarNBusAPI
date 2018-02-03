using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class CarCompany
	{
        [Key, ForeignKey("CompanyOf")]
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public Car CompanyOf { get; set; }
    }
}
