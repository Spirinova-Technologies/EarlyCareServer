using System;

namespace EarlyCare.WebApi.Models
{
    public class PlasmaRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public bool IsRtpcrReportAvailable { get; set; }
        public bool IsAntibodyReportAvailable { get; set; }
        public DateTime? CovidPositiveDate { get; set; }
        public DateTime? CovidNegativeDate { get; set; }
        public bool IsVerified { get; set; }
        public int UserId { get; set; }
        public string DonorType { get; set; }
        public int CityId { get; set; }
    }
}