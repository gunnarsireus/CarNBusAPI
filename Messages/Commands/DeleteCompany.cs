using System;
namespace Messages.Commands
{

    [Serializable]
    public class DeleteCompany 
    {
        public DeleteCompany()
        {
            DataId = Guid.NewGuid();
        }
		public Guid DataId { get; set; }
		public Guid CompanyId { get; set; }
	}
}
