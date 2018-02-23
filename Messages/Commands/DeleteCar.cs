using System;
namespace Messages.Commands
{
    public class DeleteCar
    {
        public DeleteCar()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public long DeleteTimeStamp { get; set; }
    }
}
