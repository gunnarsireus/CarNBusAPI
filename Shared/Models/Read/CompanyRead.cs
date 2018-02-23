﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Shared.Models.Read
{
    public class CompanyRead
    {
        public CompanyRead()
        {
            CreationTime = DateTime.Now.ToString(new CultureInfo("sv-SE"));
        }
        public CompanyRead(Guid companyId)
        {
            CompanyId = companyId;
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Deleted { get; set; }
        public long ChangeTimeStamp { get; set; }
    }
}
