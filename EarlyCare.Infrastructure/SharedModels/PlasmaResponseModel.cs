using System;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class PlasmaResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsRtpcrReportAvailable { get; set; }
        public bool IsAntibodyReportAvailable { get; set; }
        public DateTime CovidPositiveDate { get; set; }
        public DateTime CovidNegativeDate { get; set; }
        public bool IsVerified { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string DonorType { get; set; }
        public string City { get; set; }
    }
}