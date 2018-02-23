using System;
using System.Globalization;

namespace Shared.Models.Write
{
    public class Car
    {
        public Car()
        {
        }

        public Car(Guid carId)
        {
            CarId = carId;
            CreationTime = DateTime.Now.ToString(new CultureInfo("se-SE"));
        }

        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string VIN { get; set; }
        public string RegNr { get; set; }
    }
}
