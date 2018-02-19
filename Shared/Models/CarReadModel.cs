using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Models
{
    public class CarReadModel
    {
        public CarReadModel(Car car)
        {
            CarId = car.CarId;
            CompanyId = car.CompanyId;
            CreationTime = car.CreationTime;
            CarOnlineStatus = car._CarOnlineStatus.Online;
            CarLockedStatus = car._CarLockedStatus.Locked;
            CarSpeed = car._CarSpeed.Speed;
            VIN = car.VIN;
            RegNr = car.RegNr;        }
        [Key]
        public Guid CarId { get; set; }
        public Guid CompanyId { get; set; }
        public string CreationTime { get; set; }
        public string VIN { get; set; }
        public string RegNr { get; set; }
        public bool CarOnlineStatus { get; set; }
        public bool CarLockedStatus { get; set; }
        public int CarSpeed { get; set; }
        public Company CarOf { get; set; }
    }
}
