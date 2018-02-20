using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Models
{
    public class Car
    {
        public Car()
        {
            CreationTime = DateTime.Now.ToString(new CultureInfo("se-SE"));
            CarOnlineStatuses = new List<CarOnlineStatus>
            {
                new CarOnlineStatus { Online = true }
            };

            CarLockedStatuses = new List<CarLockedStatus>
            {
                new CarLockedStatus
                {
                    Locked = false,
                }
            };

            CarSpeeds = new List<CarSpeed>
            {
                new CarSpeed
                {
                    Speed = 576
                }
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
        public List<CarOnlineStatus> CarOnlineStatuses { get; set; }
        [Required]
        public List<CarLockedStatus> CarLockedStatuses { get; set; }
        [Required]
        public List<CarSpeed> CarSpeeds { get; set; }
        public Company CarOf { get; set; }
    }
}
