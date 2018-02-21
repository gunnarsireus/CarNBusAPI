using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Read
{
    public class CarRead
    {
        public CarRead()
        {

        }
        public CarRead(Guid carId)
        {
            CarId = carId;
        }
        [Key]
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string VIN { get; set; }
        public string RegNr { get; set; }
        public bool Online { get; set; }
        public bool Locked { get; set; }
        public long LockedTimeStamp { get; set; }
        public int Speed { get; set; }
    }
}
