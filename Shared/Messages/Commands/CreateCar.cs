using System;
namespace Shared.Messages.Commands
{
    public class CreateCar 
    {
        public CreateCar()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CarId { get; set; }
		public string CreationTime { get; set; }
		public string VIN { get; set; }
		public string RegNr { get; set; }
        public Guid CompanyId { get; set; }
        public bool Online { get; set; }
        public bool Locked { get; set; }
        public int Speed { get; set; }
        public long  CreateCarTimeStamp { get; set; }
    }
}