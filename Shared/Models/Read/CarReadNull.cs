using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Read
{
    public class CarReadNull
    {
        public CarReadNull()
        {
            Id = Guid.NewGuid();
        }
        public CarReadNull(Guid carId, Guid companyId):this()
        {
            CarId = carId;
            CompanyId = companyId;
        }
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string VIN { get; set; }
        public string RegNr { get; set; }
        public bool? Online { get; set; }
        public bool? Locked { get; set; }
        public long LockedTimeStamp { get; set; }
        public int? Speed { get; set; }
        public bool? Deleted { get; set; }
        public long ChangeTimeStamp { get; set; }
    }
}
