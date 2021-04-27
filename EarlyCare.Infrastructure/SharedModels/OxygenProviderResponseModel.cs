using System;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class OxygenProviderResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PhoneNumber { get; set; }
        public string Charges { get; set; }
        public string GovRegistraionNumber { get; set; }
        public string Type { get; set; }
        public bool IsVerified { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string City { get; set; }
    }
}