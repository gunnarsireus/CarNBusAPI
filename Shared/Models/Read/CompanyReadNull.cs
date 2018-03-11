﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Shared.Models.Read
{
    public class CompanyReadNull
    {
        public CompanyReadNull()
        {
            CreationTime = DateTime.Now.ToString(new CultureInfo("sv-SE"));
            Id = Guid.NewGuid();
        }
        public CompanyReadNull(Guid companyId): this()
        {
            CompanyId = companyId;
        }
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? Deleted { get; set; }
        public long ChangeTimeStamp { get; set; }
    }
}