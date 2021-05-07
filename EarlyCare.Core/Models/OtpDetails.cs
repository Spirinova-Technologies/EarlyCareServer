using System;

namespace EarlyCare.Core.Models
{
    public class OtpDetails
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string Otp { get; set; }
        public bool IsVerified { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}