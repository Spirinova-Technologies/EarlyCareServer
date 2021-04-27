using System;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class AmbulanceResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsVerified { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string AmbulanceType { get; set; }
        public string ProviderType { get; set; }
        public string City { get; set; }
    }
}