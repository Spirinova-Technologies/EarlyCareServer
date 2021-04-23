using System;

namespace EarlyCare.Infrastructure.SharedModels
{
    public class ConsultationResponseModel
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string Area { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Fee { get; set; }
        public string TimeSlots { get; set; }
        public bool IsVerified { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ConsultationType { get; set; }
        public string City { get; set; }
    }
}