using System;
namespace Messages.Commands
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
    }
}
