﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Models
{
    public class Car
    {
        public Car()
        {
            CarId = Guid.NewGuid();
            CreationTime = DateTime.Now.ToString(new CultureInfo("se-SE"));
            _CarOnlineStatus = new CarOnlineStatus
            {
                Online = true,
                CarId = CarId
            };
            _CarLockedStatus = new CarLockedStatus
            {
                Locked = false,
                CarId = CarId
            };
        }

        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }

        [Display(Name = "Created date")]
        public string CreationTime { get; set; }

        [Display(Name = "VIN (VehicleID)")]
        [RegularExpression(@"^[A-Z0-9]{6}\d{11}$", ErrorMessage = "{0} denoted as X1Y2Z300001239876")]
        [Remote("VinAvailableAsync", "Car", ErrorMessage = "VIN already taken")]
        public string VIN { get; set; }

        [Display(Name = "Reg. Nbr.")]
        [RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "{0} denoted as XYZ123")]
        [Remote("RegNrAvailableAsync", "Car", ErrorMessage = "Registration number taken")]
        public string RegNr { get; set; }
        [Required]
        public CarOnlineStatus _CarOnlineStatus { get; set; }
        [Required]
        public CarLockedStatus _CarLockedStatus { get; set; }
        public Company CarOf { get; set; }
    }
}
