using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Shared.Models.Write
{
    public class Car
    {
        public Car()
        {
            CreationTime = DateTime.Now.ToString(new CultureInfo("se-SE"));
        }

        public Car(Guid carId):this()
        {
            CarId = carId;
        }
        [Key]
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string VIN { get; set; }
        public string RegNr { get; set; }
    }
}
