using System;

namespace Messages.Commands
{
    public class CreateCarSpeed
    {
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public int Speed { get; set; }
        public long CreationTime { get; set; }
    }
}