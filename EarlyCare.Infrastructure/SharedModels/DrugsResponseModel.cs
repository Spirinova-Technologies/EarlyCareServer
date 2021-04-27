using System;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class DrugsResponseModel
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Coordinator { get; set; }
        public string GovPhoneNumber { get; set; }
        public int CityId { get; set; }
        public bool IsVerified { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    }
}