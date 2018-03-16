using System;

namespace Shared.Messages.Commands
{
    public class UpdateCompany 
    {
        public UpdateCompany()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid Id { get; set; }
		public string CreationTime { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
	}
}
