using System;

namespace EarlyCare.Core.Models
{
    public class OtpDetails
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Mobile { get; set; }
        public string Otp { get; set; }
        public bool IsVerified { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}