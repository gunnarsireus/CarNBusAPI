using NServiceBus;
using System;
using System.Collections.Generic;
namespace Messages.Commands
{
    [Serializable]
    public class CreateCompany: IMessage
	{
        public CreateCompany()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid Id { get; set; }
		public string CreationTime { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public ICollection<Guid> Cars { get; set; }
	}
}
