using System;

namespace Shared.Models
{
    public class ClientCar
    {
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string VIN { get; set; }
        public string RegNr { get; set; }
        public bool Online { get; set; }
        public bool Locked { get; set; }
        public int Speed { get; set; }
    }
}
