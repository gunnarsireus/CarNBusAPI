using System;
namespace Messages.Commands
{
    public class UpdateCarSpeed 
    {
        public UpdateCarSpeed()
        {
            DataId = Guid.NewGuid();
        }
        public Guid DataId { get; set; }
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public int Speed { get; set; }
        public long UpdateCarSpeedTimeStamp { get; set; }
    }
}
