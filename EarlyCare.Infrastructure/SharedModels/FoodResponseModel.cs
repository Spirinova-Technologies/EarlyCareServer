using System;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class FoodResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PhoneNumber { get; set; }
        public string Charges { get; set; }
        public string RegistrationNumber { get; set; }
        public string DeliveryType { get; set; }
        public string FoodProviderType { get; set; }
        public bool IsVerified { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string City { get; set; }
    }
}