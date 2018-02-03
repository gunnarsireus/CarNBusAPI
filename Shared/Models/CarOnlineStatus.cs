﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class CarOnlineStatus
	{
        [Key, ForeignKey("OnlineStatusOf")]
        public Guid CarId { get; set; }
        public bool Online { get; set; }
        public Car OnlineStatusOf { get; set; }

	}
}
