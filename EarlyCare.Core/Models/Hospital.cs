using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyCare.Core.Models
{
   public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string PhoneNumber3 { get; set; }
        public string PhoneNumber4 { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        public string HospitalType { get; set; }
        public int VacantIsolation { get; set; }
        public int VacantWithOxygen { get; set; }
        public int VacantWithICU { get; set; }
        public int VacantWithICUVentilator { get; set; }
        public int TotalIsolation { get; set; }
        public int TotalWithOxygen { get; set; }
        public int TotalWithICU { get; set; }
        public int TotalWithICUVentilator { get; set; }
        public bool IsAvailableIsolationBed { get; set; }
        public bool IsAvailableICUVentilator { get; set; }
        public bool IsAvailableICU { get; set; }
        public bool IsAvailableOxygen { get; set; }
        public bool IsSynced { get; set; }
    }
}
