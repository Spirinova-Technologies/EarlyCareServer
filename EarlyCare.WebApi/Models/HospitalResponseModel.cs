using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Models
{
    public class HospitalResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string City { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string PhoneNumber3 { get; set; }
        public string PhoneNumber4 { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string HospitalType { get; set; }
        public int VacantIsolation { get; set; }
        public int VacantWithOxygen { get; set; }
        public int VacantWithICU { get; set; }
        public int VacantWithICUVentilator { get; set; }
        public int TotalIsolation { get; set; }
        public int TotalWithOxygen { get; set; }
        public int TotalWithICU { get; set; }

        public int TotalWithICUVentilator { get; set; }
    }
}
