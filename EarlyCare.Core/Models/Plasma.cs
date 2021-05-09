using System;

namespace EarlyCare.Core.Models
{
    public class Plasma
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public bool IsRtpcrReportAvailable { get; set; }
        public bool IsAntibodyReportAvailable { get; set; }
        public DateTime? CovidPositiveDate { get; set; }
        public DateTime? CovidNegativeDate { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string DonorType { get; set; }
        public int CityId { get; set; }
        public bool IsSynced { get; set; }
    }
}