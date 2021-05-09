using System;

namespace EarlyCare.Core.Models
{
    public class Ambulance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string AmbulanceType { get; set; }
        public string NumberOfAmbulances { get; set; }
        public string ProviderType { get; set; }
        public int CityId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsSynced { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}